import { Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  messageContent = '';

  constructor(public messageService: MessageService) {}

  ngOnInit() {
    this.messageService.messageThread$.subscribe({
      next: messages => {
        console.log(messages.length);
        console.log(messages);
      }
    })
  }

  sendMessage() {
    if (this.username) {
      this.messageService.sendMessage(this.username, this.messageContent).then(() => {
        this.messageForm?.reset();
      })
    }
    this.messageService.messageThread$.subscribe({
      next: messages => {
        console.log('after sending');
        console.log(messages.length);
        console.log(messages);
      }
    })
  }
}
