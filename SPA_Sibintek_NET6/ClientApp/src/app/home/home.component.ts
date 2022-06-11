import {Component, ViewChild} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {Md5} from 'ts-md5/dist/md5';
import {saveAs} from 'file-saver';
import {MatSnackBar} from "@angular/material/snack-bar";
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public files!: UserFile[];
  public canUpload!: boolean;
  public displayedColumns: string[] = ['name', 'uploadDateTime', 'download', 'delete'];
  public dataSource!: MatTableDataSource<UserFile>;

  private selectedFile!: File;
  private selectedFileMD5!: string;

  private http: HttpClient;
  private snackBar: MatSnackBar

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(http: HttpClient, snackBar: MatSnackBar) {
    this.snackBar = snackBar;
    this.http = http;
    http.get<UserFile[]>('/api/v1/Files/').subscribe(result => {
      this.files = result;
      this.setTableDataSource();
    }, error => this.openErrorSnackBar(error));
  }

  public async onFileSelected($event: any): Promise<void> {
    const file = $event.target.files[0] as File;

    if(file.name.length > 300){
      this.openErrorSnackBar('Имя файла слишком длинное.')
      return;
    }

    const fileBytes = new Uint8Array(await file.arrayBuffer());
    const md5 = this.checkMD5(fileBytes);

    if (this.files.find(f => f.hash === md5) == null) {
      this.canUpload = true;
      this.selectedFile = file;
      this.selectedFileMD5 = md5;
    } else {
      this.openErrorSnackBar('Этот файл уже загружен.');
      this.canUpload = false;
    }
  }

  public async uploadFile(): Promise<void> {
    this.canUpload = false;

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('contentType', this.selectedFile.type);
    formData.append('md5', this.selectedFileMD5);

    this.http.post('api/v1/Files/Add/', formData).subscribe(result => {
        this.openInfoSnackBar('Файл загружен.');

        this.files.push(result as UserFile);
        this.setTableDataSource();
      }
      , error => {
        this.canUpload = true;
        console.log(JSON.stringify(error));
        this.openErrorSnackBar(error);
      });
  }

  public downloadFile(file: UserFile) {
    this.http.get('api/v1/Files/Download/' + file.id, {responseType: 'blob'}).subscribe(result => {
      saveAs(result, file.name)
    }, error => {
      this.openErrorSnackBar(error);
    });
  }

  public deleteFile(file: UserFile) {
    this.http.delete('api/v1/Files/Delete/' + file.id).subscribe(result => {
      this.openInfoSnackBar('Файл удален.');
      this.removeFileFormArray(file);
    }, error => {
      this.openErrorSnackBar(error);
    });
  }

  private removeFileFormArray(file: UserFile) {
    const index = this.files.indexOf(file, 0);
    if (index > -1) {
      this.files.splice(index, 1);
    }
  }

  private checkMD5(bytes: Uint8Array): string {
    const md5 = new Md5();
    md5.appendByteArray(bytes);
    return md5.end() as string;
  }

  private openErrorSnackBar(message: string) {
    this.snackBar.open(message, 'Закрыть');
  }

  private openInfoSnackBar(message: string) {
    this.snackBar.open(message)._dismissAfter(3 * 1000);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  private setTableDataSource() {
    this.dataSource = new MatTableDataSource(this.files);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}

export interface UserFile {
  id: number;
  name: string;
  hash: string;
  uploadDateTime: Date;
}
