import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule for ngClass and other common directives
import { FormsModule } from '@angular/forms';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-chat',
  standalone: true,  // Mark it as standalone
  imports: [CommonModule, FormsModule],  // Add CommonModule and FormsModule to imports
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  @ViewChild('anchor', { static: false }) anchor!: ElementRef;

  messages: ChatEntry[] = [];
  modelInput: string = '';
  messageInput: string = '';
  isAsking: boolean = false;
  isConnected: boolean = false;

  private hubConnection!: HubConnection;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    // Check if user is authenticated
    if (!this.authService.isAuthenticated()) {
      alert("Not authenticated");
      return;
    }
  }

  async connect(): Promise<void> {
    const token = this.authService.getToken(); // Get JWT from sessionStorage

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7021/chatStreamingHub', {
        accessTokenFactory: () => token ?? '' // Attach JWT to SignalR connection
      })
      .build();

    await this.hubConnection.start();
    if (this.hubConnection.state === 'Connected') {
      this.isConnected = true;
    }

    await this.hubConnection.send('CreateClient', this.modelInput);
  }

  async ask(): Promise<void> {
    if (this.hubConnection && !this.isAsking) {
      this.isAsking = true;
      this.messages.push(new ChatEntry('User', this.messageInput));

      const responseMessage = new ChatEntry('Assistant', '');

      const responseStream = this.hubConnection.stream('SendCompletion', this.messageInput);
      this.messageInput = '';

      this.messages.push(responseMessage);
      this.scrollIntoView();

      responseStream.subscribe({
        next: (responseChunk: string) => {
          responseMessage.content += responseChunk;
          this.scrollIntoView();
        },
        complete: () => (this.isAsking = false),
        error: (err: any) => {
          console.error(err);
          this.isAsking = false;
        }
      });
    }
  }

  scrollIntoView(): void {
    setTimeout(() => this.anchor.nativeElement.scrollIntoView({ behavior: 'smooth' }), 0);
  }
}

class ChatEntry {
  constructor(public sender: string, public content: string) {}
}
