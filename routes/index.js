const router = require("express").Router();

router.use("/users", require("./users"));

router.use("/blogs", require("./blogs"));

router.use("/blogs", require("./tags"));

router.use("/blogs", require("./posts"));

// /blogs

module.exports = router;
