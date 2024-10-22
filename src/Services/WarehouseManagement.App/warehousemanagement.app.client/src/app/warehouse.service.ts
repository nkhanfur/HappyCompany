import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Warehouse } from '../models/warehouse.model';

@Injectable({
  providedIn: 'root'
})
export class WarehouseService {
  private apiUrl = 'http://localhost:5249/api/v1/warehouses'; 

  constructor(private http: HttpClient) { }

  // Fetch all warehouses with pagination
  getWarehouses(page: number, pageSize: number): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(
      `${this.apiUrl}?pageNumber=${page}&pageSize=${pageSize}`
    );
  }

  getWarehouseById(id: number): Observable<Warehouse> {
    return this.http.get<Warehouse>(`${this.apiUrl}/${id}`);
  }

  addWarehouse(warehouse: Warehouse): Observable<Warehouse> {
    return this.http.post<Warehouse>(this.apiUrl, warehouse);
  }
}
