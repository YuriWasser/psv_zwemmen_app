CREATE TABLE `lookup_functie` (
  `code` varchar(20) PRIMARY KEY,
  `beschrijving` varchar(100)
);

CREATE TABLE `zwembad` (
  `id` integer PRIMARY KEY,
  `naam` varchar(50),
  `adres` varchar(100)
);

CREATE TABLE `gebruiker` (
  `id` integer PRIMARY KEY,
  `gebruikersnaam` nvarchar(40),
  `wachtwoord` nvarchar(40),
  `email` nvarchar(40),
  `voornaam` nvarchar(40),
  `achternaam` nvarchar(40),
  `functie_code` varchar(20)
);

CREATE TABLE `competitie` (
  `id` integer PRIMARY KEY,
  `naam` varchar(100),
  `start_datum` date,
  `eind_datum` date,
  `zwembad_id` integer
);

CREATE TABLE `programma` (
  `id` integer PRIMARY KEY,
  `competitie_id` integer,
  `omschrijving` varchar(50),
  `datum` date,
  `start_tijd` time
);

CREATE TABLE `afstand` (
  `id` integer PRIMARY KEY,
  `meters` integer,
  `beschrijving` varchar(50)
);

CREATE TABLE `training` (
  `id` integer PRIMARY KEY,
  `zwembad_id` integer,
  `datum` date,
  `tijd` time
);

CREATE TABLE `afstand_per_programma` (
  `programma_id` integer,
  `afstand_id` integer,
  PRIMARY KEY (`programma_id`, `afstand_id`)
);

CREATE TABLE `inschrijving` (
  `gebruiker_id` integer,
  `programma_id` integer,
  `afstand_id` integer,
  `inschrijfdatum` date,
  PRIMARY KEY (`gebruiker_id`, `programma_id`, `afstand_id`)
);

CREATE TABLE `resultaat` (
  `id` integer PRIMARY KEY,
  `gebruiker_id` integer,
  `programma_id` integer,
  `afstand_id` integer,
  `tijd` time
);

CREATE TABLE `clubrecord` (
  `id` integer PRIMARY KEY,
  `gebruiker_id` integer,
  `afstand_id` integer,
  `record` time,
  `datum` date
);

CREATE TABLE `feedback` (
  `id` integer PRIMARY KEY,
  `gebruiker_id` integer,
  `programma_id` integer,
  `feedback_text` nvarchar(255)
);

CREATE TABLE `gebruiker_training` (
  `gebruiker_id` integer,
  `training_id` integer,
  PRIMARY KEY (`gebruiker_id`, `training_id`)
);

ALTER TABLE `gebruiker` ADD FOREIGN KEY (`functie_code`) REFERENCES `lookup_functie` (`code`);

ALTER TABLE `competitie` ADD FOREIGN KEY (`zwembad_id`) REFERENCES `zwembad` (`id`);

ALTER TABLE `programma` ADD FOREIGN KEY (`competitie_id`) REFERENCES `competitie` (`id`);

ALTER TABLE `afstand_per_programma` ADD FOREIGN KEY (`programma_id`) REFERENCES `programma` (`id`);

ALTER TABLE `afstand_per_programma` ADD FOREIGN KEY (`afstand_id`) REFERENCES `afstand` (`id`);

ALTER TABLE `inschrijving` ADD FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruiker` (`id`);

ALTER TABLE `inschrijving` ADD FOREIGN KEY (`programma_id`) REFERENCES `programma` (`id`);

ALTER TABLE `inschrijving` ADD FOREIGN KEY (`afstand_id`) REFERENCES `afstand` (`id`);

ALTER TABLE `training` ADD FOREIGN KEY (`zwembad_id`) REFERENCES `zwembad` (`id`);

ALTER TABLE `resultaat` ADD FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruiker` (`id`);

ALTER TABLE `resultaat` ADD FOREIGN KEY (`programma_id`) REFERENCES `programma` (`id`);

ALTER TABLE `resultaat` ADD FOREIGN KEY (`afstand_id`) REFERENCES `afstand` (`id`);

ALTER TABLE `clubrecord` ADD FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruiker` (`id`);

ALTER TABLE `clubrecord` ADD FOREIGN KEY (`afstand_id`) REFERENCES `afstand` (`id`);

ALTER TABLE `feedback` ADD FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruiker` (`id`);

ALTER TABLE `feedback` ADD FOREIGN KEY (`programma_id`) REFERENCES `programma` (`id`);

ALTER TABLE `gebruiker_training` ADD FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruiker` (`id`);

ALTER TABLE `gebruiker_training` ADD FOREIGN KEY (`training_id`) REFERENCES `training` (`id`);
