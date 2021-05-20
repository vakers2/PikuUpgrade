import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import { Link } from 'react-router-dom';

export class Rooms extends Component {
  static displayName = Rooms.name;

  constructor(props) {
    super(props);
    this.state = { rooms: [], loading: true };
  }

  componentDidMount() {
    this.populateRoomsData();
  }

  static renderRoomsTable(rooms) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Host</th>
            <th>Players</th>
          </tr>
        </thead>
        <tbody>
          {rooms.map(room =>
            <tr key={room.name}>
              <td><Link to={`/room/${room.id}`}>{room.name}</Link></td>
              <td>{room.host}</td>
              <td>{room.numberOfPlayers}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Rooms.renderRoomsTable(this.state.rooms);

    return (
      <div>
        <h1 id="tabelLabel" >Rooms available</h1>
        {contents}
      </div>
    );
  }

  async populateRoomsData() {
    const token = await authService.getAccessToken();
    const response = await fetch('rooms', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ rooms: data, loading: false });
  }
}
