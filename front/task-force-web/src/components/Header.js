import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const Header = ({ onCreateTask }) => {
  return (
    <header className="navbar navbar-light bg-light justify-content-between px-3">
      <h1 className="navbar-brand">TaskForce</h1>
      <button className="btn btn-primary" onClick={onCreateTask}>
        Create Task
      </button>
    </header>
  );
};

export default Header;