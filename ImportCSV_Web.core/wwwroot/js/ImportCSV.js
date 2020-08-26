//servername dropdownlist

var servername = new Vue({
    el: '#servername',
    data: {
        server: [{
            "id": "1",
            "Name": "PRODConnection",
        },
        {
            "id": "2",
            "Name": "UATConnection",
        },
        {
            "id": "3",
            "Name": "SITConnection",
        },
        {
            "id": "4",
            "Name": "DEVConnection",
        },
        {
            "id": "5",
            "Name": "DEFAULTConnection",
        }],
        Server: -1,
        Serverdrp: "Select"
    },
    methods: {
        selectedvalue: function (select) {
            this.Serverdrp = select;
        }
    }
})



Vue.component('demo-grid', {
    template: "#grid-template",
    props: {
        data: Array,
        columns: Array
    }
});
var demo = new Vue({
    el: '#demo',
    data: {
        gridColumns: ['name', 'power'],
        gridData: [
            { name: 'Chuck Norris', power: Infinity },
            { name: 'Bruce Lee', power: 9000 },
            { name: 'Jackie Chan', power: 7000 },
            { name: 'Jet Li', power: 8000 }
        ]
    }
})

