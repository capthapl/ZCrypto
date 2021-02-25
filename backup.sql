--
-- PostgreSQL database dump
--

-- Dumped from database version 10.16
-- Dumped by pg_dump version 10.16

-- Started on 2021-02-24 15:51:17

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12924)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2832 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 198 (class 1259 OID 16418)
-- Name: buy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.buy (
    id integer NOT NULL,
    coin_id character varying(255) NOT NULL,
    exchange_rate_pln numeric(10,2) NOT NULL,
    count numeric(10,2) NOT NULL,
    created_time date DEFAULT CURRENT_DATE NOT NULL,
    active boolean DEFAULT true NOT NULL
);


ALTER TABLE public.buy OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16416)
-- Name: buy_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.buy_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.buy_id_seq OWNER TO postgres;

--
-- TOC entry 2833 (class 0 OID 0)
-- Dependencies: 197
-- Name: buy_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.buy_id_seq OWNED BY public.buy.id;


--
-- TOC entry 196 (class 1259 OID 16394)
-- Name: coin; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coin (
    id character varying(255) NOT NULL,
    name character varying(255) NOT NULL,
    symbol character varying(255) NOT NULL,
    rank integer NOT NULL,
    is_new boolean NOT NULL,
    is_active boolean NOT NULL,
    type character varying(255) NOT NULL
);


ALTER TABLE public.coin OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 16445)
-- Name: coin_blacklist_integration; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coin_blacklist_integration (
    id integer NOT NULL,
    coin_id character varying(255),
    reason character varying(255)
);


ALTER TABLE public.coin_blacklist_integration OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16443)
-- Name: coin_blacklist_integration_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.coin_blacklist_integration_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.coin_blacklist_integration_id_seq OWNER TO postgres;

--
-- TOC entry 2834 (class 0 OID 0)
-- Dependencies: 201
-- Name: coin_blacklist_integration_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.coin_blacklist_integration_id_seq OWNED BY public.coin_blacklist_integration.id;


--
-- TOC entry 200 (class 1259 OID 16432)
-- Name: reported_exchange_rate; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reported_exchange_rate (
    id integer NOT NULL,
    buy_id integer NOT NULL,
    pln_exchange double precision NOT NULL,
    api_update_time timestamp with time zone NOT NULL,
    exchange_diff double precision NOT NULL
);


ALTER TABLE public.reported_exchange_rate OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16430)
-- Name: reported_exchange_rate_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reported_exchange_rate_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.reported_exchange_rate_id_seq OWNER TO postgres;

--
-- TOC entry 2835 (class 0 OID 0)
-- Dependencies: 199
-- Name: reported_exchange_rate_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reported_exchange_rate_id_seq OWNED BY public.reported_exchange_rate.id;


--
-- TOC entry 2688 (class 2604 OID 16421)
-- Name: buy id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.buy ALTER COLUMN id SET DEFAULT nextval('public.buy_id_seq'::regclass);


--
-- TOC entry 2692 (class 2604 OID 16448)
-- Name: coin_blacklist_integration id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coin_blacklist_integration ALTER COLUMN id SET DEFAULT nextval('public.coin_blacklist_integration_id_seq'::regclass);


--
-- TOC entry 2691 (class 2604 OID 16435)
-- Name: reported_exchange_rate id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reported_exchange_rate ALTER COLUMN id SET DEFAULT nextval('public.reported_exchange_rate_id_seq'::regclass);


--
-- TOC entry 2696 (class 2606 OID 16424)
-- Name: buy buy_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.buy
    ADD CONSTRAINT buy_pkey PRIMARY KEY (id);


--
-- TOC entry 2700 (class 2606 OID 16450)
-- Name: coin_blacklist_integration coin_blacklist_integration_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coin_blacklist_integration
    ADD CONSTRAINT coin_blacklist_integration_pkey PRIMARY KEY (id);


--
-- TOC entry 2694 (class 2606 OID 16401)
-- Name: coin coin_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coin
    ADD CONSTRAINT coin_pkey PRIMARY KEY (id);


--
-- TOC entry 2698 (class 2606 OID 16437)
-- Name: reported_exchange_rate reported_exchange_rate_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reported_exchange_rate
    ADD CONSTRAINT reported_exchange_rate_pkey PRIMARY KEY (id);


--
-- TOC entry 2701 (class 2606 OID 16425)
-- Name: buy buy_coin_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.buy
    ADD CONSTRAINT buy_coin_id_fkey FOREIGN KEY (coin_id) REFERENCES public.coin(id);


--
-- TOC entry 2703 (class 2606 OID 16451)
-- Name: coin_blacklist_integration coin_blacklist_integration_coin_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coin_blacklist_integration
    ADD CONSTRAINT coin_blacklist_integration_coin_id_fkey FOREIGN KEY (coin_id) REFERENCES public.coin(id);


--
-- TOC entry 2702 (class 2606 OID 16438)
-- Name: reported_exchange_rate reported_exchange_rate_buy_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reported_exchange_rate
    ADD CONSTRAINT reported_exchange_rate_buy_id_fkey FOREIGN KEY (buy_id) REFERENCES public.buy(id);


-- Completed on 2021-02-24 15:51:17

--
-- PostgreSQL database dump complete
--

