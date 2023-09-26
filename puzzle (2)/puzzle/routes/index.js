var express = require('express');
const multer = require('multer');
var router = express.Router();
var storage = multer.diskStorage({
    destination: function (req, file, cb) {
      cb(null, 'public/images')
    },
    filename: function (req, file, cb) {
      cb(null, file.originalname)
    }
  })
  var upload = multer({ storage: storage })

var{ add_cat, puzzle, view_puzzle, get_all, view_pzl_Bycat_id, update }=require('../controller/controller')

router.post('/add_category', upload.single('image'),add_cat);
router.post('/add_puzzle',upload.single('image'),puzzle)
router.get('/all',get_all);
router.get('/puzzleBycat/:cat_id',view_pzl_Bycat_id)
router.get('/puzzle/:plz_id',view_puzzle)
router.post('/update/:id',update)
module.exports = router;
