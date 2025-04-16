import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  constructor(public api: ApiService) {}

  async getAuthorizeUser(): Promise<any> {
    const result = await this.api.get(`User`);
    const user: any = result.data;
    return user
  }
}
