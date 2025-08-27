export interface UserRegister {
  fullName: string;
  email: string;
  phone?: string;
  password: string;
  area?: string;
  estate?: string;
  role: string;
}

export interface UserLogin {
  email: string;
  password: string;
}

export interface User {
  id: number;
  fullName: string;
  email: string;
  phone?: string;
  area?: string;
  estate?: string;
  role: string;
}

export interface UserUpdate {
  fullName: string;
  email: string;
  phone?: string;
  area?: string;
  estate?: string;
  role: string;
}
