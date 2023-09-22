var mongoose = require('mongoose');
mongoose.connect('mongodb://127.0.0.1:27017/puzzle_game')
  .then(() => console.log('Connected!'));

var add_cat = new mongoose.Schema({
   cat_name:{
    type:String
   },
   Image:{
    type:String
   }

})
module.exports = mongoose.model('category',add_cat);