import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { NgIf } from '@angular/common';
import { PasswordValidatorService } from '../../services/password-validator.service';
import { NavbarComponent } from "../../components/navbar/navbar.component";

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',  imports: [FormsModule, RouterModule, ReactiveFormsModule, NgIf, NavbarComponent],

  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {

  menuSeleccion: 'login' | 'register' = 'login';

  email: string = '';
  password: string = '';
  rememberMe: boolean = false;
  jwt: string = '';

  registerForm: FormGroup;

  pressedEnter : Boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private passwordValidator: PasswordValidatorService,
    private renderer: Renderer2
  ) {
    this.registerForm = this.formBuilder.group({
      nickname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    },
      { validators: this.passwordValidator.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.renderer.addClass(document.body, 'login-background');
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.body, 'login-background');
  }


  async login() {
    if(this.pressedEnter) return;
    this.pressedEnter = true;

    const authData = { email: this.email, password: this.password };
    const result = await this.authService.login(authData, this.rememberMe);

    if (result.success) {

      if (result.data) {
        this.jwt = result.data.accessToken;
      } else {
        console.error('No se encontró información en result.data');
      }

      console.log(this.rememberMe)

      if (this.rememberMe) {
        localStorage.setItem('jwtToken', this.jwt);
      }

      alert("Inicio sesion exitoso");

      this.router.navigateByUrl(""); // redirige a inicio
    } else {
      alert("error al iniciar sesion")
      this.pressedEnter = false

    }
  }


  // Registro
  async register() {
    if(this.pressedEnter) return;
    console.log(this.registerForm.value)
    this.pressedEnter = true

    if (this.registerForm.valid) {

      const authData = { email: this.registerForm.value.email, password: this.registerForm.value.password, name: this.registerForm.value.nickname, id: 0 };

      const registerResult = await this.authService.register(authData);

      if (registerResult.success) {

        const formUser = this.registerForm.value;

        const authData = { email: formUser.email, password: formUser.password };
        const loginResult = await this.authService.login(authData, false);

        if (loginResult.success) {
          alert("Te has registrado con éxito.")
          this.router.navigateByUrl(""); // redirige a inicio
        } else {
          alert("Error en el inicio de sesión");
          this.pressedEnter = false
        }

      } else {
        alert("Error en el registro");
        this.pressedEnter = false;
      }

    } else {
      alert("Formulario no válido");
      this.pressedEnter = false;
    }
  }

}

