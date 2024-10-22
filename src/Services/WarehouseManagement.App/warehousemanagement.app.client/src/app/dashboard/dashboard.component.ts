import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  warehouses = [
    { name: 'Warehouse 1', inventoryCount: 150 },
    { name: 'Warehouse 2', inventoryCount: 90 }
  ];

  topHighItems = [
    { name: 'Item A', quantity: 500 },
    { name: 'Item B', quantity: 400 }
  ];
}
