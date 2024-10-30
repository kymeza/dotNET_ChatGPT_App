import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommandService {
  private apiUrl = 'https://localhost:7021/api/vulnerable'; // Replace with your actual API URL

  constructor(private http: HttpClient) { }

  runCommand(command: string): Observable<string> {
    return this.http.post(this.apiUrl + '/run', { command }, { responseType: 'text' });
  }
}
