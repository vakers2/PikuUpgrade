import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import authService from './api-authorization/AuthorizeService';

export class Room extends Component {
  static displayName = Room.name;

  constructor(props) {
    super(props);
    this.state = {
      firstUrl: 'https://www.youtube.com/embed/dQw4w9WgXcQ', secondUrl: 'https://www.youtube.com/embed/dQw4w9WgXcQ', isAuthenticated: false, userName: null, messages: [], roomId: this.props.match.params.roomId, room: null, players: [], loading: true, connection: null,
    };
  }

  async componentDidMount() {
    await this.populateState();
    if (this.state.isAuthenticated) {
      this.joinRoom();
    }
  }

  static renderRoom(state) {
    return (
      <div>
        <div className="container-video">
          <iframe id="video-first" width="1111" height="625" src={state.firstUrl} title="YouTube video player" frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>
          <iframe id="video-second" width="1111" height="625" src={state.secondUrl} title="YouTube video player" frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>
        </div>

        <input id="url-first" type="text" style={{ display: 'none' }} onChange={(e) => { this.sendFirstUrl(e); }} value={state.firstUrl}/>
        <input id="url-second" type="text" style={{ display: 'none' }} onChange={(e) => { this.sendSecondUrl(e); }} value={state.secondUrl}/>
      </div>
    );
  }

  render() {
    const contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Room.renderRoom(this.state);

    return (
      <div>
        <h1 id="tabelLabel">Room: {this.state.room?.name || this.state.roomId}</h1>
        {contents}
      </div>
    );
  }

  async joinRoom() {
    const newConnection = new HubConnectionBuilder().withUrl(`/roomHub/${this.state.roomId}`).build();
    this.setState({ connection: newConnection }, () => {
      this.state.connection
        .start()
        .then(() => {
          console.log('Connection started!');
          this.setHubCallbacks(this.state.connection);
        })
        .catch((err) => console.log('Error while establishing connection :('));
    });
  }

  setHubCallbacks(connection) {
    connection.on('Notify', (message) => {
      console.log(message);
    });

    connection.on('ReceiveFirst', function (url) {
      this.setState({ firstUrl: url });
    });

    connection.on('ReceiveSecond', function (url) {
      this.setState({ secondUrl: url });
    });

    connection.invoke('Enter', this.state.roomId, this.state.userName);
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
    const token = await authService.getAccessToken();
    const response = await fetch(`room/get/${this.state.roomId}`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    const data = await response.json();
    this.setState({
      room: data,
      loading: false,
      isAuthenticated,
      userName: user && user.name,
    });
  }

  sendFirstUrl(event) {
    const { connection } = this.state;
    connection.invoke('SendFirstVideoUrl', this.state.roomId, event.value);
  }

  sendSecondUrl(event) {
    console.log('check');
    const { connection } = this.state;
    connection.invoke('SendSecondVideoUrl', this.state.roomId, event.value);
  }
}
