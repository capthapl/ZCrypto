import logo from "./logo.svg";
import React from "react";
import "./App.css";
import axios from "axios";
import CoinRow from "./CoinRow";

export default class App extends React.Component {
  BASE_API_URL = "http://SECRET/ZCrypto";

  constructor(props) {
    super(props);

    this.state = {
      initiedBuys: [],
    };
  }
  componentDidMount() {
    setInterval(() => {
      this.init();
    }, 10000);
    this.init();
  }

  init() {
    this.getActiveBuys().then((data) => {
      let elements = [];
      for (let b in data) {
        elements.push(data[b]);
      }
      this.setState({
        initiedBuys: [],
      },()=>{
        this.setState({
          initiedBuys: elements,
        });
      })
    });
  }

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
  };

  render() {
    return (
      <div className="App">
        <header>ZCrypto</header>
        <div id="content">
          <div id="coinList">
            {this.state.initiedBuys.map((element) => {
              return <CoinRow coin={element} key={element.id} />;
            })}
          </div>
        </div>
        <footer></footer>
      </div>
    );
  }
}
