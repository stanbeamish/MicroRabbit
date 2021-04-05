import React, {Component} from 'react';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import { Menu } from 'semantic-ui-react';
import Home from '../pages/Home';
import BankAccounts from '../pages/BankAccounts';
import Transfers from '../pages/Transfers';

export default class TopMenu extends Component {
    state = {};

    handleItemClick = (e, { name }) => this.setState({ activeItem: name });
    
    render() {
        const { activeItem } = this.state;

        return (
            <Router>
                <Menu fixed='top' color='red' inverted> 
                    <Link to='/'><Menu.Item 
                        name='home'
                        active={activeItem === 'home'}
                        content='Home'
                        link='/'
                        onClick={this.handleItemClick}
                    ></Menu.Item>
                    </Link>
                    <Link to='/bankaccounts'><Menu.Item 
                        name='bankaccounts'
                        active={activeItem === 'bankaccounts'}
                        content='Bank Accounts'
                        link='/bankaccounts'
                        onClick={this.handleItemClick}
                    />
                    </Link>
                    <Link to='/transfers'>
                    <Menu.Item 
                        name='transfers'
                        active={activeItem === 'transfers'}
                        content='Transfers'
                        link='/transfers'
                        onClick={this.handleItemClick}
                    />
                    </Link>
                </Menu>

                <Switch>
                    <Route exact path='/'>
                        <Home />
                    </Route>
                    <Route path='/bankaccounts'>
                        <BankAccounts />
                    </Route>
                    <Route path='/transfers'>
                        <Transfers />
                    </Route>
                </Switch>
            </Router>
        );
    }
}
