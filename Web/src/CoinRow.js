import React, { useEffect, useState } from "react";

export default ({ coin }) => {
    console.log("Init new coin")
  const [coinData, setCoin] = useState(coin);
  const [priceColor, setColor] = useState("");

  useEffect(()=>{
      isUp();
  })

  const toDateString = (d) => {
    const date = d.toISOString().split("T")[0];
    const time = d.toTimeString().split(" ")[0];
    return `${date} ${time}`;
  };

  const getDiffPrice = () => {
      console.log("diff price: " + JSON.stringify(coinData) )
    if (coinData.reportedExchangeRates.length > 0) {
      return (
        Math.round(coinData.reportedExchangeRates[0].exchangeDiff * 1000000) /
        1000000
      );
    } else {
      return "B/D";
    }
  };

  const isUp = () => {
    if (coinData.reportedExchangeRates.length > 0) {
      let val =
        Math.round(coinData.reportedExchangeRates[0].exchangeDiff * 1000000) /
        1000000;
      if (val > 0) {
        setColor("greenCoin");
      } else if (val < 0) {
        setColor("redCoin");
      }
    } else {
        setColor("greenCoin");
    }
  };

  const getLastReportedExchange = () => {
    if (coinData.reportedExchangeRates.length > 0) {
      return (
        Math.round(coinData.reportedExchangeRates[0].plnExchange * 1000000) /
        1000000
      );
    } else {
      return "B/D";
    }
  };

  const getBuyPrice = () => {
      return coinData.exchangeRatePln * coinData.count 
  }
  const getBuyExchange = () => {
    return coinData.exchangeRatePln
  };

  return (
    <div className="coinBuyContainer">
      <div className={"coinCol bold"}>Coin: {coinData.coin.name} ({coinData.coin.symbol})</div>
      <div className={"coinCol"}>Buy date: {toDateString(new Date(coinData.createdTime))}</div>
      <div className={"coinCol"}>Buy exchange: {getBuyExchange()}</div>
      <div className={"coinCol"}>Buy value: {getBuyPrice()}</div>
      <div className={"coinCol"}>Current exchange: {getLastReportedExchange()}</div>
      <div className={"coinCol "+priceColor}>Profit: {getDiffPrice()}</div>
    </div>
  );
};
