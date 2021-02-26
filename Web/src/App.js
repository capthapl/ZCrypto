import logo from "./logo.svg";
import React from "react";
import "./App.css";
import axios from "axios";
import CoinRow from "./CoinRow";
import Autocomplete from "react-autocomplete";
export default class App extends React.Component {
  BASE_API_URL = "http://secret/ZCrypto";

  constructor(props) {
    super(props);

    this.state = {
      initiedBuys: [],
      addCoinExchangeValue: "",
      addCoinCountValue: "",
      addCoinIdValue: "",
      fetchedCoinIds: [],
      visibleFetchedCoins: [],
      addCoinEnabled: true,
      securityCode: "",
    };
  }
  componentDidMount() {
    this.init();
    this.getActiveCoinIds();
  }

  init() {
    this.getActiveBuys()
      .then((data) => {
        let elements = [];
        for (let b in data) {
          elements.push(data[b]);
        }
        this.setState(
          {
            initiedBuys: [],
          },
          () => {
            this.setState({
              initiedBuys: elements,
            });
          }
        );
      })
      .finally(() => {
        setTimeout(() => {
          this.init();
        }, 2000);
      });
  }

  getActiveCoinIds = () => {
    return new Promise((resolve, reject) => {
      axios
        .get(this.BASE_API_URL + "/ActiveCoinIds")
        .then((response) => {
          this.setState({
            fetchedCoinIds: response.data,
            visibleFetchedCoins: response.data,
          });
          resolve(response.data);
        })
        .catch((e) => {
          reject(e);
        });
    });
  };

  getActiveBuys() {
    return new Promise((resolve, reject) => {
      axios
        .get(this.BASE_API_URL + "/Buy")
        .then((response) => {
          resolve(response.data);
        })
        .catch((e) => {
          reject(e);
        });
    });
  }

  getVisibleFetchedCoins = (pattern) => {
    if (pattern) {
      let newVisibleArray = [];

      this.state.fetchedCoinIds.forEach((el) => {
        if (el.toLowerCase().indexOf(pattern.toLowerCase()) !== -1) {
          newVisibleArray.push(el);
        }
        this.setState({
          visibleFetchedCoins: newVisibleArray,
        });
      });
    } else {
      this.setState({
        visibleFetchedCoins: this.state.fetchedCoinIds,
      });
    }
  };

  render() {
    return (
      <div className="App">
        <header>ZCrypto</header>
        <footer>
          <div>
            <span>Coin id </span>
            <Autocomplete
              getItemValue={(item) => item}
              items={this.state.visibleFetchedCoins}
              renderItem={(item, isHighlighted) => (
                <div
                  key={item}
                  style={{ background: isHighlighted ? "lightgray" : "white" }}
                >
                  {item}
                </div>
              )}
              value={this.state.addCoinIdValue}
              onChange={(e) => {
                this.getVisibleFetchedCoins(e.target.value);
                this.setState({
                  addCoinIdValue: e.target.value,
                });
              }}
              onSelect={(val) =>
                this.setState({
                  addCoinIdValue: val,
                })
              }
            />
          </div>
          <div>
            <span> Coin count </span>
            <input
              value={this.state.addCoinCountValue}
              onChange={(v) => {
                this.setState({
                  addCoinCountValue: v.target.value,
                });
              }}
              type="number"
              placeholder="Coin count"
            />
          </div>
          <div>
            <span>Coin exchange </span>

            <input
              type="number"
              onChange={(v) => {
                this.setState({
                  addCoinExchangeValue: v.target.value,
                });
              }}
              value={this.state.addCoinExchangeValue}
              placeholder="PLN exchange"
            />
          </div>
          <div className="minCenterForm">
            <input
              type="password"
              onChange={(v) => {
                this.setState({
                  securityCode: v.target.value,
                });
              }}
              value={this.state.securityCode}
              placeholder="Security code"
            />
            </div>
            <div className="minCenterForm">
            <button
              onClick={() => {
                this.setState(
                  {
                    addCoinEnabled: false,
                  },
                  () => {
                    axios
                      .post(
                        this.BASE_API_URL + "/AddBuy",
                        {
                          CoinId: this.state.addCoinIdValue,
                          ExchangeRatePln: this.state.addCoinExchangeValue,
                          Count: this.state.addCoinCountValue,
                          SecurityCode: this.state.securityCode,
                        },
                        {
                          headers: {
                            securityCode: this.state.securityCode,
                          },
                        }
                      )
                      .then((response) => {
                        this.setState({
                          CoinId: "",
                          ExchangeRatePln: 0,
                          Count: 0,
                          addCoinEnabled: true,
                          addCoinExchangeValue: "",
                          addCoinCountValue: "",
                          addCoinIdValue: "",
                          securityCode: "",
                        });
                      })
                      .catch((e) => {
                        this.setState({
                          addCoinEnabled: true,
                        });
                      });
                  }
                );
              }}
              disabled={!this.state.addCoinEnabled}
            >
              {" "}
              Add buy
            </button>
          </div>
        </footer>
        <div id="content">
          <div id="coinList">
            {this.state.initiedBuys.map((element) => {
              return <CoinRow coin={element} key={element.id} />;
            })}
          </div>
        </div>
      </div>
    );
  }
}
