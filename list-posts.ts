import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-list-posts',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './list-posts.html',
  styleUrl: './list-posts.css'
})
export class ListPostsComponent implements OnInit {
  private http = inject(HttpClient);

  posts: any[] = [
    { id: 6004, title: 'First Post of Angelica', body: 'This is the First by Angelica E.' },
    { id: 6003, title: 'First Post', body: 'This is my First Post' },
    { id: 6002, title: 'First Post', body: 'This is my first Post' },
    { id: 5002, title: 'First', body: 'This is my first post' },
    { id: 1, title: 'First Blog Post', body: 'This is the content for post 1.' },
    { id: 4003, title: 'Third', body: '3rd Post' },
    { id: 4002, title: 'Post', body: '2nd Post' },
    { id: 3002, title: 'First', body: 'This is my first post' },
    { id: 2002, title: 'First', body: 'This is my first post' }
  ];

  ngOnInit(): void {
    this.http.get<any[]>('http://localhost:8080/api/posts').subscribe({
      next: (data) => {
        if (data && data.length > 0) {
          this.posts = data;
        }
      },
      error: (err) => console.log('Backend not connected, using fallback mock data.', err)
    });
  }
}