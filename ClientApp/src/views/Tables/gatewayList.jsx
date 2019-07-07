import React, { Component } from 'react';
import {
    Table,
    Grid, Row, Col
} from 'react-bootstrap';

import Card from 'components/Card/Card.jsx';
import Button from 'elements/CustomButton/CustomButton.jsx';
import {
    gateway_thArray
} from 'variables/Variables.jsx';
import { Link } from 'react-router-dom';
import { reactLocalStorage } from 'reactjs-localstorage';

class gatewayList extends Component {

    constructor(props) {
        super(props);
        this.state = {
            gateways: []
        };
    }
    componentWillMount() {

        let token = reactLocalStorage.getObject('userInfo').token
        // console.log(token)

        fetch("https://localhost:5001/api/Gateways", {
            method: 'GET',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + token,
            },
            // body: JSON.stringify(temp)
        })
            .then(res => {
                if (res.status > 399 && res.status < 500) {
                    this.props.history.push('/login')
                }else return res.json()
            })
            .then(
                (result) => {
                    this.setState({
                        gateways: result
                    });
                    console.log(result)

                },
            )
    }

    renderTableData() {
        return this.state.gateways.map((gw, index) => {
            const { id, name, description } = gw //destructuring
            return (
                <tr key={id}>
                    <td>{id}</td>
                    <td>{name}</td>
                    <td>{description}</td>
                    <td className="text-left">
                    <Link to={{ pathname: '/gateways/nodes', state: { id: id} }}>
                            <a className="btn btn-simple btn-warning btn-icon edit">show</a>
                        </Link>
                    </td>
                    <td className="text-left">
                    <Link to={{ pathname: '/gateways/edit', state: { id: id} }}>
                            <a className="btn btn-simple btn-warning btn-icon edit">edit</a>
                        </Link>
                        
                    </td>
                </tr>
            )
        })
    }


    render() {
        return (
            <div className="main-content">
                <Grid fluid>
                    <Row>
                        <Col md={12}>
                            <Card
                                title="Gateway list"
                                tableFullWidth
                                content={
                                    <Table striped hover responsive>
                                        <thead>
                                            <tr>
                                                <th>#</th>

                                                {
                                                    gateway_thArray.map((prop, key) => {
                                                        return (
                                                            <th key={key}>{prop}</th>
                                                        );
                                                    })
                                                }
                                                <th>Show Nodes</th>

                                                <th>Edit</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.renderTableData()}

                                        </tbody>
                                    </Table>
                                }
                                legend={
                                    <Link to="/gateways/add" >
                                        <Button bsStyle="info" fill wd>
                                            Add Gateway
                                        </Button>
                                    </Link>
                                }
                                ftTextCenter
                            />
                        </Col>

                    </Row>
                </Grid>
            </div>
        );
    }
}

export default gatewayList;
