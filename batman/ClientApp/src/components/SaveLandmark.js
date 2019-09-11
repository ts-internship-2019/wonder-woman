import React, { Component } from 'react';

export class SaveLandmark extends Component {
    static displayName = SaveLandmark.name;

    constructor(props) {
        super(props);
        this.state = { title: "Create", newDictionaryEntry: new DictionaryData };
        this.state.newDictionaryEntry.dictionaryItemCode = 'sa';
        debugger;
        this.handleSave = this.handleSave.bind(this);
        this.handleChange = this.handleChange.bind(this);

    }

    handleSave(event) {
        event.preventDefault();
        const data = new FormData(event.target);  

        fetch('api/DictionaryLandmarkType/CreateLandmarkType', {
            method: 'POST',
            body: data,
        })
        window.top.location.reload();
    }

    handleChange(event) {
        const { name, value } = event.target;
        debugger;
        this.setState(prevState => ({ newDictionaryEntry: { ...prevState.newDictionaryEntry, dictionaryItemCode: value } }));//({ ...newDictionaryEntry: event.target.value });
    }

    render() {
        debugger;
        return <div>
            <h1>{this.state.title}</h1>
            <h3>Landmark Type</h3>
            <hr />
            <form onSubmit={this.handleSave} >
                <div>
                    <input type="hidden" name="dictionaryItemId" />
                </div>
                <div>
                    <label htmlFor="DictionaryItemCode" > DictionaryItemCode </label>
                    <div>
                        <input type="text" value={this.state.newDictionaryEntry.dictionaryItemCode} onChange={this.handleChange} />
                    </div>
                </div>
                <div>
                    <label htmlFor="DictionaryItemName" > DictionaryItemName </label>
                    <div>
                        <input type="text" name="DictionaryItemName" />
                    </div>
                </div>
                <div>
                    <label htmlFor="Description" > Description </label>
                    <div>
                        <input type="text" name="description" />
                    </div>
                </div>
                <button type="submit">Save</button>
            </form>
        </div>;
    }

}


class DictionaryData {
    dictionaryItemId: number = 0;
    dictionaryItemCode: string = "fd";
    dictionaryItemName: string = "";
    description: string = "";
}