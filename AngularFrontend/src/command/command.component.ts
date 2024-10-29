import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { CommandService } from './command.service';

@Component({
  selector: 'app-command-runner',
  standalone: true,
  templateUrl: './command.component.html',
  styleUrls: ['./command.component.css'],
  imports: [FormsModule, CommonModule]
})
export class CommandRunnerComponent {
    command: string = '';
    output: string = '';
        
    constructor(private commandService: CommandService) {}
    
    runCommand(command: string) {
        if (command) {
          this.commandService.runCommand(command).subscribe({
            next: (data) => this.output = data,
            error: (error) => {
              // If the error has a status code, it could be a more specific problem.
              if (error.status) {
                this.output = `Error ${error.status}: ${error.statusText}`;
              } else {
                this.output = 'An unexpected error occurred.';
              }
            }
          });
        } else {
          this.output = 'Please enter a command.';
        }
      }

}