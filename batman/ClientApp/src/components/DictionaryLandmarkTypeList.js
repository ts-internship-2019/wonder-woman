import React, { Component } from 'react';

export class DictionaryLandmarkTypeList extends Component {
    static displayName = DictionaryLandmarkTypeList.name;

    constructor(props) {
        super(props);
        this.state = { dictionaryList: [], loading: true };

        fetch('api/DictionaryLandmarkType/GetAllLandmarks').then(response => response.json())
            .then(data => {
                this.setState({ dictionaryList: data, loading: false });
            });
    }

    static renderDictionaryLandmarkList(dictionaryList) {
        return (
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>DictionaryItemId</th>
                        <th>DictionaryItemCode</th>
                        <th>DictionaryItemName</th>
                        <th>Description</th>
                    </tr>
                </thead>
                <tbody>
                    {dictionaryList.map(dictionaryList =>
                        <tr key={dictionaryList.dictionaryItemId}>
                            <td>{dictionaryList.dictionaryItemId}</td>
                            <td>{dictionaryList.dictionaryItemCode}</td>
                            <td>{dictionaryList.dictionaryItemName}</td>
                            <td>{dictionaryList.description}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : DictionaryLandmarkTypeList.renderDictionaryLandmarkList(this.state.dictionaryList);

        return (
            <div>
                <h1>Landmark types</h1>
                <p>This component demonstrates fetching data from the database.</p>
                {contents}
            </div>
        );
    }

}