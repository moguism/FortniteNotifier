import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { LoginRequest } from '../models/loginRequest';
import { LoginResult } from '../models/loginResult';
import { User } from '../models/user';
import { RegisterDto } from '../models/register-dto';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public readonly USER_KEY = 'user';
  public static readonly TOKEN_KEY = 'jwtToken';
  rememberMe : boolean = false

  public user: User | null = null;

  constructor(private api: ApiService) {
  }

  async login(authData: LoginRequest, rememberMe: boolean): Promise<Result<LoginResult>> { // Iniciar sesión
    const result = await this.api.post<LoginResult>('Auth/login', authData);
    this.rememberMe = rememberMe

    if (result.success && result.data) {
      const { accessToken, name } = result.data; // guardo info de la respuesta AuthResponse
      this.api.jwt = accessToken;

      if (rememberMe) { // Si se pulsó el botón recuérdame
        localStorage.setItem(AuthService.TOKEN_KEY, accessToken);
        this.saveUser(name)
      } else {
        sessionStorage.setItem(AuthService.TOKEN_KEY, accessToken);
        this.saveUser(name)
      }
    }

    return result;
  }

  saveUser(user: string)
  {
    if(this.rememberMe)
    {
      localStorage.setItem(this.USER_KEY, JSON.stringify(user));
    }
    else
    {
      sessionStorage.setItem(this.USER_KEY, JSON.stringify(user));
    }
  }

  // Comprobar si el usuario esta logeado
  isAuthenticated(): boolean {
    const token = localStorage.getItem(AuthService.TOKEN_KEY) || sessionStorage.getItem(AuthService.TOKEN_KEY);
    return !!token;
  }


  // Cerrar sesión
  async logout(): Promise<void> {
    sessionStorage.removeItem(AuthService.TOKEN_KEY);
    localStorage.removeItem(AuthService.TOKEN_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    localStorage.removeItem(this.USER_KEY);
  }


  getUser(): string | null { // Obtener datos del usuario
    const user = localStorage.getItem(this.USER_KEY) || sessionStorage.getItem(this.USER_KEY);
    return user
  }

  // Registro
  async register(register: RegisterDto): Promise<Result<any>> {
    return this.api.post<any>('Auth/register', register);
  }

}
