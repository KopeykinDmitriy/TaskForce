import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    // Пример проверки учетных данных
    if (username === '1' && password === '1') {
      login('your-token'); // Передаём токен в контекст
      navigate('/projects'); // Переход на защищённую страницу
    } else {
      alert('Неверные учетные данные');
    }
  };

  return (
    <div class="base-container">
        <div class="base-box">
            <h1 class="base-title">SIGN IN TO SCT</h1>
            <form class="base-form" onSubmit={handleSubmit}>
                <div class="input-group">
                    <label for="login">
                        <i class="fa fa-user"></i>
                    </label>
                    <input type="text" id="login" name="login" placeholder="login/email" required value={username} onChange={(e) => setUsername(e.target.value)} />
                </div>
                <div class="input-group">
                    <label for="password">
                        <i class="fa fa-lock"></i>
                    </label>
                    <input type="password" id="password" name="password" placeholder="password" required value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <button type='submit' class="btn-submit">Sign in</button>
            </form>
        </div>
    </div>
  );
};

export default LoginPage;