import {AfterViewInit, Component, ViewChild, Input} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import {UserFile} from "../home.component";
import {Observable} from "rxjs";

@Component({
  selector: 'app-files-table',
  templateUrl: './files-table.component.html',
  styleUrls: ['./files-table.component.css']
})
export class FilesTableComponent implements AfterViewInit {
  displayedColumns: string[] = ['name', 'uploadDateTime', 'download', 'delete'];
  dataSource!: MatTableDataSource<UserFile>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  @Input() Files: Observable<UserFile[]> = new Observable<UserFile[]>();

  constructor() {
    console.log(this.Files);
    this.Files.subscribe(n => this.dataSource = new MatTableDataSource(n));
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  downloadFile(row: UserFile) {

  }

  deleteFile(row: UserFile) {

  }
}
