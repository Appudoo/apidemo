var mongoose = require('mongoose');
const multer  = require('multer')
mongoose.connect('mongodb://127.0.0.1:27017/puzzle_game')
    .then(() => console.log('Connected!'));

var puzzle = new mongoose.Schema({
    cat_id: {
        type: String
    },
    puzzle_name:{
        type:String
    },
    Image:{
        type:String
    },
    word:{
        type:String
    },
    status:{
        type:Number
    }

})

module.exports = mongoose.model('puzzle',puzzle)