import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { login, authenticated } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    await login(username, password); // Передаем логин и пароль для авторизации

    console.log(authenticated);
    // Переходим на защищенную страницу после успешного логина
    if (authenticated) {
      navigate('/projects');
    }
  };

  return (
    <div className="base-container">
      <div className="base-box">
        <h1 className="base-title">SIGN IN TO SCT</h1>
        <form className="base-form" onSubmit={handleSubmit}>
          <div className="input-group">
            <label htmlFor="login">
              <i className="fa fa-user"></i>
            </label>
            <input
              type="text"
              id="login"
              name="login"
              placeholder="login/email"
              required
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div className="input-group">
            <label htmlFor="password">
              <i className="fa fa-lock"></i>
            </label>
            <input
              type="password"
              id="password"
              name="password"
              placeholder="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button type="submit" className="btn-submit">
            Sign in
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
