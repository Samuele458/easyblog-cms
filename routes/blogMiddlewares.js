const blogUtils = require("../lib/blogUtils");

function checkField(req, field, where) {
  locations = ["query", "params", "body"];

  //checking if location is valid
  if (typeof where !== "undefined" && locations.indexOf(where) === -1)
    return false;

  if (typeof where !== "undefined") {
    return req[where][field];
  } else {
    let returnValue;
    locations.forEach((loc, i) => {
      if (typeof req[loc][field] !== "undefined") returnValue = req[loc][field];
    });
    return returnValue;
  }
}

module.exports.checkBlogExists = (field, where) => {
  return (req, res, next) => {
    blogUrlName = checkField(req, field, where);

    if (!blogUrlName)
      res.status(400).json({
        success: false,
        msg: "Missing required field",
      });
    else {
      blogUtils.blogExists(blogUrlName, (err, data) => {
        if (err) res.status(err.status).json(err.response);
        else {
          if (!req.checked) req.checked = {};

          req.checked.blog = data;
          next();
        }
      });
    }
  };
};

module.exports.checkTagExists = (tagField, whereTagField) => {
  return (req, res, next) => {
    tagUrlName = checkField(req, tagField, whereTagField);

    if (!tagUrlName)
      res.status(400).json({
        success: false,
        msg: "Missing required field",
      });
    else
      blogUtils.tagExists(
        Array.isArray(tagUrlName) ? tagUrlName : [tagUrlName],
        req.checked.blog,
        (err, data) => {
          if (err) res.status(err.status).json(err.response);
          else {
            req.checked.checked = data;
            next();
          }
        }
      );
  };
};