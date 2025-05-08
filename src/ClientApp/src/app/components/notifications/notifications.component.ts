import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth-service.service';
import { NotificationDto } from '../../dtos/notification/NotificationDto';
import { AsyncPipe } from "@angular/common";
import { MatchService } from "../../services/match-service.service";
import { FormsModule } from "@angular/forms";

type NotificationWithSelection = NotificationDto & { isSelected: boolean }; // <== добавили тип расширения!

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, AsyncPipe, FormsModule],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  notifications: NotificationWithSelection[] = []; // <== теперь всё чётко
  isLoading = false;
  currentUserId: number | null = null;
  isVisible = false;
  activeTab: 'unread' | 'read' = 'unread';

  constructor(private matchService: MatchService, private authService: AuthService) {}

  ngOnInit() {
    this.authService.currentUserId$.subscribe(id => {
      if (id) {
        this.currentUserId = id;
        this.loadNotifications();
      }
    });
  }

  loadNotifications() {
    this.isLoading = true;
    this.matchService.getNotifications().subscribe(data => {
      this.notifications = data.map(notification => ({
        ...notification,
        isSelected: false
      }));
      this.isLoading = false;
    });
  }

  get filteredNotifications() {
    if (this.activeTab === 'unread') {
      return this.notifications.filter(n => !n.isRead);
    } else {
      return this.notifications.filter(n => n.isRead);
    }
  }

  markSelectedAsRead() {
    const selectedIds = this.notifications
      .filter(n => n.isSelected && !n.isRead)
      .map(n => n.id);

    if (selectedIds.length > 0) {
      this.matchService.markNotificationsAsRead(selectedIds).subscribe(() => {
        this.notifications.forEach(n => {
          if (selectedIds.includes(n.id)) {
            n.isRead = true;
            n.isSelected = false;
          }
        });
      });
    }
  }

  markAllAsRead() {
    const unreadIds = this.notifications
      .filter(n => !n.isRead)
      .map(n => n.id);

    if (unreadIds.length > 0) {
      this.matchService.markNotificationsAsRead(unreadIds).subscribe(() => {
        this.notifications.forEach(n => {
          if (unreadIds.includes(n.id)) {
            n.isRead = true;
            n.isSelected = false;
          }
        });
      });
    }
  }

  toggleNotifications() {
    this.isVisible = !this.isVisible;
    if (this.isVisible && this.currentUserId) {
      this.loadNotifications();
    }
  }
}
