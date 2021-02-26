import React, { useEffect, useState } from "react";

export default ({ coin }) => {
  const [coinData, setCoin] = useState(coin);
  const [priceColor, setColor] = useState("");

  useEffect(() => {
    isUp();
  });

  const toDateString = (d) => {
    const date = d.toISOString().split("T")[0];
    return `${date}`;
  };

  const getDiffPrice = () => {
    if (coinData.reportedExchangeRates.length > 0) {
      return (
        Math.round(coinData.reportedExchangeRates[0].exchangeDiff * 10000) /
        10000
      );
    } else {
      return "B/D";
    }
  };

  const isUp = () => {
    if (coinData.reportedExchangeRates.length > 0) {
      let val =
        Math.round(coinData.reportedExchangeRates[0].exchangeDiff * 10000) /
        10000;
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
        Math.round(coinData.reportedExchangeRates[0].plnExchange * 10000) /
        10000
      );
    } else {
      return "B/D";
    }
  };

  const getBuyPrice = () => {
    return Math.round(coinData.exchangeRatePln * coinData.count * 1000) / 1000;
  };

  const getBuyExchange = () => {
    return Math.round(coinData.exchangeRatePln * 1000) / 1000;
  };

  const percentProfit = () => {
    return (
      Math.round(
        (((getBuyPrice() + getDiffPrice()) / getBuyPrice()) * 100 - 100) * 1000
      ) / 1000
    );
  };

  const currentTotalValue = () => {
    return Math.round((getBuyPrice() + getDiffPrice()) * 1000) / 1000;
  };

  return (
    <div className="coinBuyContainer">
      <div className="coinCol col">
        <div className={"coinCol bold"}>
          Coin: {coinData.coin.name} ({coinData.coin.symbol}) x{coinData.count}
        </div>
        <div className={"coinCol"}>
          Buy date: {toDateString(new Date(coinData.createdTime))}
        </div>
      </div>
      <div className="coinCol col">
        <div className={"coinCol"}>{getBuyExchange()} PLN - Buy exchange</div>
        <div className={"coinCol"}>
          {getLastReportedExchange()} PLN - Current exchange
        </div>
      </div>
      <div className="coinCol col">
        <div className={"coinCol blue"}>{getBuyPrice()} PLN - Buy value</div>
        <div className={"coinCol " + priceColor}>
          {currentTotalValue()} PLN - Total value
        </div>
      </div>
      <div className="coinCol col">
        <div className={"coinCol " + priceColor}>
          {getDiffPrice()} PLN - Profit
        </div>
        <div className={"coinCol " + priceColor}>
          {percentProfit()}% - Percent profit
        </div>
      </div>
      {/*<div className={"coinBtn"}>Sold</div> */}
    </div>
  );
};
