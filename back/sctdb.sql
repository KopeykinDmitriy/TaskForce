CREATE DATABASE sctdb;

-- Подключаемся к бд:
\c sctdb

CREATE USER myuser WITH PASSWORD 'TriSlona';

--Создаем таблицу users:
CREATE TABLE users (
id BIGSERIAL NOT NULL PRIMARY KEY,
name VARCHAR(50) NOT NULL,
surname VARCHAR(50),
email VARCHAR(150),
password VARCHAR(50) NOT NULL
);

CREATE TABLE projects (
id BIGSERIAL NOT NULL PRIMARY KEY,
name VARCHAR(50) NOT NULL,
description TEXT
);

CREATE TABLE users_projects (
id BIGSERIAL NOT NULL PRIMARY KEY,
user_id INT NOT NULL,
project_id INT NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(id),
FOREIGN KEY (project_id) REFERENCES projects(id)
);

CREATE TABLE tasks (
id BIGSERIAL NOT NULL PRIMARY KEY,
name VARCHAR(50) NOT NULL,
description TEXT,
project_id INT NOT NULL,
dt_start TIMESTAMP NOT NULL,
dt_end TIMESTAMP NOT NULL,
tsask_status VARCHAR(50),
task_priority INT,
FOREIGN KEY (project_id) REFERENCES projects(id)
);

CREATE TABLE task_relation(
id BIGSERIAL NOT NULL PRIMARY KEY,
id_task1 INT NOT NULL,
id_task2 INT NOT NULL,
FOREIGN KEY (id_task1) REFERENCES tasks(id),
FOREIGN KEY (id_task2) REFERENCES tasks(id)
);

CREATE TABLE log(
id BIGSERIAL NOT NULL PRIMARY KEY,
task_id INT NOT NULL,
user_id INT NOT NULL,
task_component VARCHAR(50), -- я не помню что это и зачем оно нужно
last_value VARCHAR(50) NOT NULL,
new_value VARCHAR(50) NOT NULL,
log_dt TIMESTAMP NOT NULL,
FOREIGN KEY (task_id ) REFERENCES tasks(id),
FOREIGN KEY (user_id ) REFERENCES users(id)
);

CREATE TABLE tags (
id BIGSERIAL NOT NULL PRIMARY KEY,
name VARCHAR(50) NOT NULL
);

CREATE TABLE tasks_tags(
id BIGSERIAL NOT NULL PRIMARY KEY,
task_id INT NOT NULL,
tag_id INT NOT NULL,
FOREIGN KEY (task_id) REFERENCES tasks(id),
FOREIGN KEY (tag_id) REFERENCES tags(id)
);

CREATE TABLE users_tags(
id BIGSERIAL NOT NULL PRIMARY KEY,
user_id INT NOT NULL,
tag_id INT NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(id),
FOREIGN KEY (tag_id) REFERENCES tags(id)
);