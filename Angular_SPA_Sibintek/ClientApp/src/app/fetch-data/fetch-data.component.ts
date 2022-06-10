import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Md5} from 'ts-md5/dist/md5';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public files: UserFile[];
  public canUpload: boolean;

  private selectedFile: File;
  private selectedFileMD5: string;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    http.get<UserFile[]>(baseUrl + 'api/v1/Files/').subscribe(result => {
      this.files = result;
    }, error => console.error(error));
  }

  public async onFileSelected($event: any): Promise<void> {
    const file = $event.target.files[0] as File;

    const reader = new FileReader();
    reader.readAsArrayBuffer(file);

    reader.onloadend = (evt: any) => {
      if (evt.target.readyState === FileReader.DONE) {
        const arrayBuffer = evt.target.result;
        const array = new Uint8Array(arrayBuffer);

        const md5 = this.checkMD5(array);
        console.log(md5);

        this.canUpload = true;
        this.selectedFile = file;
        this.selectedFileMD5 = md5;
      }
    };

    // if (this.files.find(f => f.hash === md5) == null) {
    //   this.canUpload = true;
    //   this.selectedFile = file;
    //   this.selectedFileMD5 = md5;
    // }
    // else { this.canUpload = false; }
  }

  public uploadFile(): void {
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('md5', this.selectedFileMD5);

    // this.http.put(this.baseUrl + 'api/v1/Files/', formData).subscribe(result => {
    //   console.log('File uploaded.');
    //   this.canUpload = false;
    // }, error => console.error(error));

    this.http.get<UserFile[]>(this.baseUrl + 'api/v1/Files/').subscribe(result => {
      this.files = result;
      console.log('get files');
    }, error => console.error(error));
  }

  private checkMD5(file): string {
    const md5 = new Md5();
    md5.appendByteArray(file);
    return md5.end() as string;
  }
}

interface UserFile {
  name: string;
  hash: string;
  uploadDateTime: Date;
}
