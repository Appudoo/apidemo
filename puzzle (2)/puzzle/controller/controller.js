var usermodel = require('../user/category')
// var sub_category = require('../user/sub_categoty')
var puzzle = require('../user/puzzle')

exports.add_cat =async (req,res)=>{
    var image_name = req.file.originalname;
    req.body.Image=image_name;

    var data = await usermodel.create(req.body);
    console.log(req.body.name)
    res.status(200).json({
        status:"cat_name inserted",
        data
    })
}
exports.update = async (req,res)=>{
    var data = await puzzle.findByIdAndUpdate(req.params.id,{
        status:req.body.status
    })
    res.status(200).json(
        
        data
    )
    // console.log(data);
}
exports.add_sub = async (req,res)=>{
    var cat_id = req.params.cat_id;
    var sub_name = req.body.sub_category
    var data = await sub_category.create({
        sub_category : sub_name,
        category_id : cat_id
    });
    
    res.status(200).json({
        status:"sub_cat inserted"
    })
}

exports.get_all = async (req,res)=>{
    var data = await usermodel.find()
    res.status(200).json(  
        data
    )
    
}
puzzle.findById
exports.view_pzl_Bycat_id = async(req,res)=>{
    var cat_id = req.params.cat_id;
    var data = await puzzle.find({cat_id:cat_id})
    res.status(200).json(
        data
    )
}

exports.view_puzzle = async(req,res)=>{
    var data =await puzzle.findById(req.params.plz_id)
    res.status(200).json([
        
        data
    ])
}

exports.puzzle = async (req,res)=>{
    var sub_cat_id = req.params.sub_cat_id;
    var word = req.body.word;
    var status = req.body.status;
    var image_name = req.file.originalname;
    req.body.Image = image_name

    var data = await puzzle.create(req.body)

    res.status(200).json(
    
        data
    )
}