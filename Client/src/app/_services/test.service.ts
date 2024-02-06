import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TestEntity } from '../_models/testEntity';
import { environment } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class TestService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<TestEntity[]>(`${this.apiUrl}test`);
  }
}
