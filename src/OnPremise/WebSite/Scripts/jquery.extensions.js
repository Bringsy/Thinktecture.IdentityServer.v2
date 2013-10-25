
// ------------------------------------------------------------------------------------------------------------
// JavaScript String extension. parse json string to javascript date
// TODO: move to dateFormat script 
String.prototype.dateFromJson = function () {
    return eval(this.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
};

String.prototype.tryParseDate = function () {
    try {
        return this.dateFromJson();
    } catch (e) {
        return new Date(this);
    }
};

String.prototype.trim = function () { return this.replace(/^\s\s*/, '').replace(/\s\s*$/, ''); };

String.prototype.ltrim = function() { return this.replace(/^\s+/, ''); }; 

String.prototype.rtrim = function() { return this.replace(/\s+$/, ''); }; 

String.prototype.fulltrim = function() { return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' '); }; 

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

(function ($) {

    $.fn.alphanumeric = function (p) {
        p = $.extend({
            ichars: "!@#$%^&*()+=[]\\\';,/{}|\":<>?~`.- ",
            nchars: '',
            allow: ''
        }, p);

        return this.each(function () {
            if (p.nocaps)
                p.nchars += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

            if (p.allcaps)
                p.nchars += 'abcdefghijklmnopqrstuvwxyz';

            s = p.allow.split('');
            for (i = 0; i < s.length; i++)
                if (p.ichars.indexOf(s[i]) != -1) s[i] = "\\" + s[i];
            p.allow = s.join('|');

            var reg = new RegExp(p.allow, 'gi');
            var ch = p.ichars + p.nchars;
            ch = ch.replace(reg, '');

            $(this).keypress(function (e) {
                if (!e.charCode)
                    k = String.fromCharCode(e.which);
                else
                    k = String.fromCharCode(e.charCode);

                if (ch.indexOf(k) != -1)
                    e.preventDefault();


                if (e.ctrlKey && k == 'v')
                    e.preventDefault();
            });

            $(this).bind('contextmenu', function () { return false });
        });
    };

    $.fn.numeric = function (p) {
        var az = 'abcdefghijklmnopqrstuvwxyz';
        az += az.toUpperCase();

        p = $.extend({
            nchars: az
        }, p);

        return this.each(function() {
            $(this).alphanumeric(p);
        }); 
    };

    $.fn.alpha = function (p) {
        var nm = '1234567890';

        p = $.extend({
            nchars: nm
        }, p);

        return this.each(function() {
            $(this).alphanumeric(p);
        }); 
    };

    // ------------------------------------------------------------------------------------------------------------
    // Defuscator all mailto links,
    // need a serverside obfuscator yRides.RideSharing.Web.HelperExtensions.Obfuscate(String)
    $.fn.defuscate = function () {
        var $el = $(this);
        var c = './0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~';

        $el.find('a[href^="mailto:"]').each(function () {
            var href = '';
            $.each($(this).attr('href').substr(7), function (i, v) {
                href += c.charAt(c.indexOf(v) - 1);
            });
            $(this).attr('href', 'mailto:' + href);

            var html = '';
            $.each($(this).html(), function (i, v) {
                html += c.charAt(c.indexOf(v) - 1);
            });
            $(this).html(html);
        });

        return $el;
    };

    // ------------------------------------------------------------------------------------------------------------
    // disables a ui-button with smoth animations
    $.fn.disableSmooth = function (callback) {
        return $(this).animate({ opacity: 0.35 }, 200, function(evt) {
            $(this).addClass('ui-state-disabled').attr('disabled', true);
            if ($.isFunction(callback))
                callback();
        }); 
    };

    // ------------------------------------------------------------------------------------------------------------
    // enables a ui-button with smoth animations
    $.fn.enableSmooth = function (callback) {
        return $(this).animate({ opacity: 1 }, 200, function(evt) {
            $(this).removeAttr('disabled').removeClass('ui-state-disabled');
            if ($.isFunction(callback))
                callback();
        }); 
    };

    // ------------------------------------------------------------------------------------------------------------
    // swaps a value of an ui-button with smoth animations
    $.fn.swapVal = function (val, callback) {
        return $(this).animate({ opacity: 0.35 }, 200, function (evt) {
            $(this).val(val).animate({ opacity: 1 }, 100, function (evt) {
                if ($.isFunction(callback))
                    callback();
            });
        });
    };

    // ------------------------------------------------------------------------------------------------------------
    // maxZ jQuery Plugin set a Z-Index of an element to top 
    $.fn.maxZ = function (opt) {
        var def = { inc: 10, group: "*" };
        $.extend(def, opt);
        var zmax = 0;
        $(def.group).each(function () {
            var cur = parseInt($(this).css('z-index'));
            zmax = cur > zmax ? cur : zmax;
        });

        if (!this.jquery)
            return zmax;

        return this.each(function () {
            zmax += def.inc;
            $(this).css("z-index", zmax);
        });
    };

    $.extend({

        tryParseStr: function (v, d) {
            if (d == undefined)
                d = '&nbsp;';

            if (v == undefined || v == null)
                return d;

            if (v.length == 0)
                return d;

            return v;
        },

        tryParseNr: function (v, d) {
            if (d == undefined)
                d = 0;

            if (v == undefined || v == null)
                return d;

            return v;
        },

        // concat the array to a url query 
        toUrlParams: function (obj) {
            var p = new Array();
            for (var k in obj) {
                if (obj[k] !== undefined && obj[k] !== null) {
                    var v = obj[k];
                    if (isNaN(v))
                        v = escape(v);
                    p.push(k + '=' + v);
                }
            }

            if (p.length > 0)
                return p.join('&');
            else
                return '';
        },

        // get all url parameters
        getUrlParams: function () {
            var rtn = {}, sp = window.location.hash.split('?');
            if (sp.length <= 1)
                return rtn;

            var args = sp[1].split('&');
            for (var i = 0; i < args.length; i++) {
                var arg = args[i].split('='), v = arg[1];
                if (isNaN(v))
                    v = unescape(v);

                rtn[arg[0]] = v;
            }

            return rtn;
        },


        // ------------------------------------------------------------------------------------------------------------
        // get the url paramater
        getUrlParam: function (key) {
            return decodeURI(
                (RegExp('[?|&]' + key + '=' + '(.+?)(&|$)').exec(location) || [, null])[1]
            );
        },

        // stringify fallback 
        stringify: window.JSON ? JSON.stringify : function (b) {
            if (typeof b == "string") {
                return '"' + b.replace(/\\/g, "\\\\").replace(/"/g, '\\"').replace(/\n/g, "\\n").replace(/\r/g, "\\r") + '"';
            } else {
                if (typeof b == "number") {
                    return b.toString();
                } else {
                    if (b == null) {
                        return "null";
                    } else {
                        if (b.length != undefined) {
                            return "[" + $.map(b, stringify).join(",") + "]";
                        }
                    }
                }
            }
            var a = "{";
            for (var c in b) {
                a = a + stringify(c) + ":" + stringify(b[c]) + ",";
            }
            if (a.length > 1) {
                a = a.substr(0, a.length - 1);
            }
            return a + "}";
        },



        // ------------------------------------------------------------------------------------------------------------
        // stores data in localStorage if HTML5 is enabled or in a cookie  
        storeData: function (key, data) {
            if (!!window.localStorage) {
                window.localStorage.setItem(key, $.stringify(data));
            }
            else {
                $.cookie(key, $.stringify(data));
            }
        },

        // ------------------------------------------------------------------------------------------------------------
        // reads data from localStorage if HTML5 is enabled or from a cookie  
        restoreData: function (key) {
            if (!!window.localStorage) {
                var a = window.localStorage.getItem(key);
                if (a) {
                    return $.parseJSON(a);
                }
                return null;
            }
            else {
                return $.parseJSON($.cookie(key));
            }
        },

        removeData: function (key) {
            if (!!window.localStorage) {
                window.localStorage.removeItem(key);
            }
            else {
                return $.cookie(key, null);
            }
        }
    });

})(jQuery);
