Vor der Inbetriebnahmen des Servers sind folgende SQL Befehle auf einem PostgreSQL Server auszuführen:

-- Database: MTCG

-- DROP DATABASE IF EXISTS "MTCG";

CREATE DATABASE "MTCG"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'German_Germany.1252'
    LC_CTYPE = 'German_Germany.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Table: public.players

-- DROP TABLE IF EXISTS public.players;

CREATE TABLE IF NOT EXISTS public.players
(
    name character varying(32) COLLATE pg_catalog."default" NOT NULL,
    password character varying(64) COLLATE pg_catalog."default" NOT NULL,
    coins integer NOT NULL DEFAULT 20,
    collection integer NOT NULL DEFAULT 0,
    elo integer NOT NULL DEFAULT 100,
    win integer NOT NULL DEFAULT 0,
    loss integer NOT NULL DEFAULT 0,
    deck integer[] DEFAULT ARRAY['-1'::integer, 0, 0, 0],
    CONSTRAINT players_pkey PRIMARY KEY (name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.players
    OWNER to postgres;


-- Table: public.trades

-- DROP TABLE IF EXISTS public.trades;

CREATE TABLE IF NOT EXISTS public.trades
(
    tid SERIAL NOT NULL,
    name character varying(32) COLLATE pg_catalog."default" NOT NULL,
    offered integer NOT NULL,
    wantsc integer NOT NULL,
    wantsm integer NOT NULL,
    CONSTRAINT "Trades_pkey" PRIMARY KEY (tid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.trades
    OWNER to postgres;


-- Table: public.boosterpacks

-- DROP TABLE IF EXISTS public.boosterpacks;

CREATE TABLE public.boosterpacks
(
    id serial NOT NULL,
    cards integer[] NOT NULL,
    PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.boosterpacks
    OWNER to postgres;



-- PROCEDURE: public.setupcontent()

-- DROP PROCEDURE IF EXISTS public.setupcontent();

CREATE OR REPLACE PROCEDURE public.setupcontent(
	)
LANGUAGE 'sql'
AS $BODY$
TRUNCATE TABLE  players RESTART IDENTITY;
TRUNCATE TABLE  trades RESTART IDENTITY;
TRUNCATE TABLE  boosterpacks RESTART IDENTITY;
INSERT INTO  boosterpacks
(
	cards
)
VALUES

	('{0,1,2,3,4}'),
	('{5,6,7,8,9}'),
	('{10,11,12,13,14}'),
	('{15,16,17,18,19}'),
	('{20,21,22,23,24}'),
	('{25,26,27,28,29}'),
	('{5,10,15,20,25}'),
	('{6,11,16,21,26}'),
	('{7,12,17,22,27}'),
	('{8,13,18,23,28}'),
	('{9,14,19,24,29}');
$BODY$;

CALL public.setupcontent();
