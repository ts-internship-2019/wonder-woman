import React, { Component } from 'react';

export class SaveLandmark extends Component {
    static displayName = SaveLandmark.name;

    constructor(props) {
        super(props);
        this.state = { title: "Create", newDictionaryEntry: new DictionaryData};
        this.handleSave = this.handleSave.bind(this);
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

    render() {
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
                        <input type="text" name="dictionaryItemCode" />
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
    dictionaryItemCode: string = "";
    dictionaryItemName: string = "";
    description: string = "";
}