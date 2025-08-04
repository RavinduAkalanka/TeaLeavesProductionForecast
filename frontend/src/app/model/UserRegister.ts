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

