import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadFileService {
  private apiUrl = 'https://localhost:7021/api/vulnerable'; // Replace with your actual API URL

  constructor(private http: HttpClient) { }

  uploadFile(file: File, fileName: string): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName); // Append 'fileName' to the FormData

    return this.http.post(`${this.apiUrl}/upload`, formData, {
      reportProgress: true, // Optional: if you want to track progress
      observe: 'events'     // Optional: if you want to receive HttpEvents
    });
  }

}
