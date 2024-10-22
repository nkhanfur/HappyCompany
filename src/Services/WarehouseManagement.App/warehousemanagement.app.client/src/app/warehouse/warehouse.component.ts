import { Component, OnInit } from '@angular/core';
import { WarehouseService } from '../services/warehouse.service';
import { Warehouse } from '../models/warehouse.model';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css'],
})
export class WarehouseComponent implements OnInit {
  warehouses: Warehouse[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  totalPagesArray: number[] = [];

  constructor(private warehouseService: WarehouseService) { }

  ngOnInit(): void {
    this.loadWarehouses();
  }

  // Fetch warehouses for the current page
  loadWarehouses(): void {
    this.warehouseService.getWarehouses(this.currentPage, this.pageSize).subscribe((data) => {
      this.warehouses = data;
      // Assume total pages are returned via API (you can implement this in the backend)
      this.totalPages = Math.ceil(100 / this.pageSize); // Mock total count of 100 for now
      this.totalPagesArray = Array(this.totalPages)
        .fill(0)
        .map((_, i) => i + 1);
    });
  }

  // Change the current page
  changePage(page: number): void {
    if (page > 0 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadWarehouses();
    }
  }

  // View warehouse details
  viewWarehouse(id: number): void {
    console.log('View warehouse', id);
  }

  // Add a new warehouse
  addWarehouse(): void {
    console.log('Add new warehouse');
  }
}
