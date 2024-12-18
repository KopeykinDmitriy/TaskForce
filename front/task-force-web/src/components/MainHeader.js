import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getUserInfo } from '../services/api';

const MainHeader = () => {
  const [user, setUser] = useState({ name: 'UnknownUser', role: 'user' });

  useEffect(() => {
    const getUser = async () => {
      const userInfo = await getUserInfo();
      setUser(userInfo);
    };
    getUser();
  }, []);

  return (
    <div
      style={{
        backgroundColor: '#3a7bd5',
        color: 'white',
        padding: '20px',
        textAlign: 'center',
        fontFamily: 'Arial, sans-serif',
        position: 'relative',
      }}
    >
      <img
        src="/images/logo-bg.jpg"
        alt="Logo"
        style={{
          position: 'absolute',
          left: '30px',
          top: '40%',
          transform: 'translateY(-50%)',
          height: '90px',
        }}
      />

      <div>
        <h1>Hello, {user.name}!</h1>
        <p>Welcome to Smart Collaboration Tools</p>
        <p>
          Choose your project{' '}
          {user.role === 'admin' && (
            <Link to="/create-new-project">
              <button
                style={{
                  backgroundColor: '#ff6600',
                  color: 'white',
                  border: 'none',
                  padding: '5px 10px',
                  cursor: 'pointer',
                  borderRadius: '5px',
                  marginLeft: '10px',
                }}
              >
                or create new
              </button>
            </Link>
          )}
        </p>
      </div>

      <Link to="/logout">
        <button
          style={{
            position: 'absolute',
            top: '20px',
            right: '20px',
            width: '80px',
            height: '40px',
            backgroundColor: '#ffffff',
            border: 'none',
            cursor: 'pointer',
            boxShadow: '0px 4px 6px rgba(0, 0, 0, 0.2)',
            borderRadius: '5px',
          }}
        >
          Выйти
        </button>
      </Link>
    </div>
  );
};

export default MainHeader;
