import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/Sidebar.css'

const Sidebar = () => {
  return (
    <div className="sidebar">
      <div class="sidebar-top">
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/projects">Home</Link></button>
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/">Kanban</Link></button>
      </div>
      <div class="sidebar-bot">
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/settings/users">Users</Link></button>
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/logout">Logout</Link></button>
      </div>
    </div>
  );
};

export default Sidebar;
