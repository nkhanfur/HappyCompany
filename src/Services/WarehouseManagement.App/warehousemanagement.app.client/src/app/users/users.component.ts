import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  users = [
    { id: 1, username: 'admin', role: 'Admin' },
    { id: 2, username: 'manager', role: 'Management' },
    { id: 3, username: 'auditor', role: 'Auditor' }
  ];

  ngOnInit() { }

  onEditUser(userId: number) {
    // Redirect to edit user page
  }

  onChangePassword(userId: number) {
    // Redirect to change password page
  }

  onDeleteUser(userId: number) {
    // Delete user action
  }
}

