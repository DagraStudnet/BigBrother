CREATE TABLE Db_observer(
  id_observer     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  first_name   TEXT, 
  last_name TEXT 
);

CREATE TABLE Db_event(
  id_event     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  event_name   TEXT 
);

CREATE TABLE Db_date_time_event(
  id_date_time_event     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,   
  start_event TEXT,
  end_event TEXT,
  id_observer INTEGER NOT NULL,  
  id_event INTEGER NOT NULL,
  id_user INTEGER,
 FOREIGN KEY(id_observer) REFERENCES Db_observer(id_observer) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY(id_user) REFERENCES Db_user(id_user)ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY(id_event) REFERENCES Db_event(id_event)ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Db_user_timestamp(
  id_user_timestamp     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,   
  user_timestamp TEXT,  
  id_user INTEGER, 
 FOREIGN KEY(id_user) REFERENCES Db_user(id_user)ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Db_user(
  id_user     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  user_name   TEXT, 
  pc_name TEXT   
);

CREATE TABLE Db_activity(
  id_activity     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  name   TEXT, 
  time_activity TEXT,
  attention BOOL,
  id_user INTEGER,  
  FOREIGN KEY(id_user) REFERENCES Db_user(id_user)ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Db_user_work(
  id_user_work     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  name_work   TEXT,  
  id_user INTEGER, 
  id_event INTEGER,  
  FOREIGN KEY(id_user) REFERENCES Db_user(id_user)ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY(id_event) REFERENCES Db_event(id_event)ON DELETE CASCADE ON UPDATE CASCADE
);

 