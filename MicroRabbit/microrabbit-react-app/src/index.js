import React from 'react';
import ReactDOM from 'react-dom';
import 'semantic-ui-css/semantic.min.css';
import { Container } from 'semantic-ui-react';
import Heading from './components/Heading';
import TopMenu from './components/TopMenu';

if (module.hot) {
    module.hot.accept();
}

const App = () => {
    return (
        <Container>
            <TopMenu />                      
        </Container>
    )
}

ReactDOM.render(<App/>, document.querySelector('#root'));

