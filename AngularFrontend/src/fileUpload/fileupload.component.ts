import { Component } from '@angular/core';
import { UploadFileService } from './fileupload.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
    selector: 'app-uploadfile',
    standalone: true,
    templateUrl: './fileupload.component.html',
    styleUrls: ['./fileupload.component.css'],
    imports: [FormsModule, CommonModule]
})
export class UploadFileComponent {
    fileToUpload: File | null = null;
    fileName: string = '';
    uploadStatus: string = '';
    constructor(private uploadService: UploadFileService) { }
    uploadFile() {
        if (!this.fileToUpload) {
            this.uploadStatus = 'Please select a file to upload.';
            return;
        }

        this.uploadService.uploadFile(this.fileToUpload, this.fileName).subscribe({
            next: (event) => {
                // Handle a HttpEventType.UploadProgress event (optional)
                if (event.type === HttpEventType.UploadProgress && event.total) {
                    const progress = Math.round(100 * event.loaded / event.total);
                    this.uploadStatus = `File is ${progress}% uploaded.`;
                } else if (event instanceof HttpResponse) {
                    this.uploadStatus = 'File uploaded successfully!';
                }
            },
            error: (error) => {
                this.uploadStatus = `Error: ${error.statusText} (${error.status})`;
            }
        });
    }

    onFileSelected(event: any) {
        this.fileToUpload = event.target.files[0];
    }

}