var sa_config = {
    portalBaseServiceUrl: '/_vti_bin/xPortal/PortalService.svc',
    reuestDigest: '',
    userProfile: {}
};

; (function ($)
{
    $.fn.datepicker.dates['tr'] = {
        days: RESOURCE.CLIENT_Day_Names,
        daysShort: RESOURCE.CLIENT_Day_Short_Names,
        daysMin: RESOURCE.CLIENT_Day_Min_Names,
        months: RESOURCE.CLIENT_Mouth_Names,
        monthsShort: RESOURCE.CLIENT_Mouth_Short_Names,
        today: RESOURCE.CLIENT_Today_Text,
        format: "dd.mm.yyyy",
        clear: RESOURCE.CLIENT_Clear_Text
    };
}(jQuery));


(function ()
{
    'use strict';
    var queryString = {};

    queryString.parse = function (str)
    {
        if (typeof str !== 'string')
        {
            return {};
        }

        str = str.trim().replace(/^\?/, '');

        if (!str)
        {
            return {};
        }

        return str.trim().split('&').reduce(function (ret, param)
        {
            var parts = param.replace(/\+/g, ' ').split('=');
            var key = parts[0];
            var val = parts[1];

            key = decodeURIComponent(key);
            // missing `=` should be `null`:
            // http://w3.org/TR/2012/WD-url-20120524/#collect-url-parameters
            val = val === undefined ? null : decodeURIComponent(val);

            if (!ret.hasOwnProperty(key))
            {
                ret[key] = val;
            } else if (Array.isArray(ret[key]))
            {
                ret[key].push(val);
            } else
            {
                ret[key] = [ret[key], val];
            }

            return ret;
        }, {});
    };

    queryString.stringify = function (obj)
    {
        return obj ? Object.keys(obj).map(function (key)
        {
            var val = obj[key];

            if (Array.isArray(val))
            {
                return val.map(function (val2)
                {
                    return encodeURIComponent(key) + '=' + encodeURIComponent(val2);
                }).join('&');
            }

            return encodeURIComponent(key) + '=' + encodeURIComponent(val);
        }).join('&') : '';
    };

    queryString.push = function (key, new_value)
    {
        var params = queryString.parse(location.search);
        params[key] = new_value;
        var new_params_string = queryString.stringify(params)
        history.pushState({}, "", window.location.pathname + '?' + new_params_string);
    }

    if (typeof module !== 'undefined' && module.exports)
    {
        module.exports = queryString;
    } else
    {
        window.queryString = queryString;
    }
})();

var innTools = innTools || {};
innTools = {
    pageInit: PageInit,
    JSONSerializer: function ()
    {
        return Sys.Serialization.JavaScriptSerializer.serialize(arguments[0]);
    },
    JSONDeserializer: function ()
    {
        return Sys.Serialization.JavaScriptSerializer.deserialize(arguments[0]);
    },
    GetUserProfile: function ()
    {
        var _params = arguments[0] || {};
        var _uri = _params.accountName ? String.format("/_api/SP.UserProfiles.PeopleManager/GetPropertiesFor(accountName=@v)?@v='{0}'", _params.accountName) : "/_api/SP.UserProfiles.PeopleManager/GetMyProperties";
        $.get(_uri, {}, function (data, textStatus, jqXHR)
        {
            sa_config.userProfile = data.d;
            _params.callback.call(true, data, textStatus, jqXHR);
        });
    },
    getURLParameterByName: function (name)
    {
        var url = arguments[1] || location.search;
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(url);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    getParameterByKey: function ()
    {
        var _properties = arguments[0] || {},
            _keys = arguments[0] || [],
            _list = [];

        $.each(_properties, function (i, item)
        {
            if (item.Key)
            {
                _list.push(item);
            }
        });
        return _list;
    },
    isDate: function (str)
    {
        var parms = str.split(/[\.\-\/]/);
        var yyyy = parseInt(parms[2], 10);
        var mm = parseInt(parms[1], 10);
        var dd = parseInt(parms[0], 10);
        var date = new Date(yyyy, mm - 1, dd, 12, 0, 0, 0);
        return mm === (date.getMonth() + 1) &&
            dd === date.getDate() &&
            yyyy === date.getFullYear();
    },
    handleIEFixes: function ()
    {
        //fix html5 placeholder attribute for ie7 & ie8
        if (jQuery.browser.msie && jQuery.browser.version.substr(0, 1) < 9)
        { // ie7&ie8
            jQuery('input[placeholder], textarea[placeholder]').each(function ()
            {
                var input = jQuery(this);

                jQuery(input).val(input.attr('placeholder'));
                jQuery(input).focus(function ()
                {
                    if (input.val() == input.attr('placeholder'))
                    {
                        input.val('');
                    }
                });
                jQuery(input).blur(function ()
                {
                    if (input.val() == '' || input.val() == input.attr('placeholder'))
                    {
                        input.val(input.attr('placeholder'));
                    }
                });
            });
        }
    },
    siteNames: function ()
    {
        var lang = _spPageContextInfo.currentCultureName,
            langShort = lang.substring(0, 2),
            sites = {
                EN: {
                    id: langShort,
                    Pages: "Pages",
                    News: "news",
                    Announcements: "announcements",
                    Surveys: "surveys",
                    Search: "search",
                    GroupCompany: "GroupCompany",

                },
                TR: {
                    id: langShort,
                    Pages: "Sayfalar",
                    News: "haberler",
                    Announcements: "duyurular",
                    Surveys: "anketler",
                    Search: "arama",
                    GroupCompany: "topluluksirketleri"
                }
            };

        switch (lang)
        {
            case "en-US":
                return sites.EN;
                break;
            case "tr-TR":
                return sites.TR;
                break;
            default:
                return sites.TR;
                break;
        }
    }
};
function PageInit()
{
    sa_config.reuestDigest = $('#__REQUESTDIGEST').val();
    $.ajaxSetup({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            "X-RequestDigest": sa_config.reuestDigest,
            "accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose"
        }
    });

    $(document).on('click', '.dropdown-menu .ddm-content', function (e) { e.stopPropagation(); });
    //$('.desk-icons a[title]').tooltip();
    //$('.desk-icons a[title]').tooltip('show');
    $('.selectpicker').selectpicker();

    $('#HomePageLayout .home-friend-feed .content').ClassyScroll({ autoHide: true, sliderOpacity: .1 });
    //$('#featuredControl .ms-webpart-zone').ClassyScroll({ autoHide: true, sliderOpacity: .1 });

    //#region SearchInputs
    var _searchInputInProfile = $('#ProfileInfo_SmallSearchInputBox input[type=text]');
    var _searchLinkInProfile = $('#ProfileInfo_SmallSearchInputBox .ms-srch-sb-searchLink');
    var _searchInputInMenu = $('#Menu_SmallSearchInputBox input[type=text]');
    var _searchLinkInMenu = $('#Menu_SmallSearchInputBox .ms-srch-sb-searchLink');

    var _searchRefferer = function (e)
    {
        if (e.keyCode === 13 || e.type === 'click')
        {
            var _val = (e.keyCode === 13) ? $(e.currentTarget).val() : $(e.currentTarget).prev('input[type=text]').val();
            if (_val != null && _val[_val.length - 1] != '*')
            {
                _val += '*';
            }
            window.location.href = String.format("/{0}/{3}/{4}/{1}.aspx?k={2}", _spPageContextInfo.currentCultureName, e.data.page, _val, innTools.siteNames().Search, innTools.siteNames().Pages);
            return false;
        }
    };

    var _searchKey = innTools.getURLParameterByName('k');
    if (_searchKey)
    {
        if (location.pathname.indexOf('peopleresults') !== -1)
        {
            _searchInputInProfile.val(_searchKey.replace("*", ""));
        }
        else
        {
            _searchInputInMenu.val(_searchKey);
        }
    }
    _searchInputInMenu.on('keydown keypress', { page: "results" }, _searchRefferer);
    _searchLinkInMenu.on('click', { page: "results" }, _searchRefferer);
    _searchInputInProfile.on('keydown keypress', { page: "peopleresults" }, _searchRefferer);
    _searchLinkInProfile.on('click', { page: "peopleresults" }, _searchRefferer);
    _searchInputInMenu.watermark();
    _searchInputInProfile.watermark();
    //#endregion InputHandler

    //isListPage
    //if (ctx && ctx.ListSchema.Toolbar === 'Standard') {
    //    $('.ms-webpart-zone').each(function () {
    //        //$('#systemContent').addClass('container-fluid');
    //        //$(this).addClass('well');
    //    });
    //    //console.info("ctx : %o", ctx);
    //}

    //ProfileViewModel();
    //$('.ms-toolbar,.ms-propertysheet').parents('#systemContent').addClass('container-fluid well');

    //console.clear();
    //console.group("Authentication phase");
    //console.info("g_wsaListTemplateId: %d", g_wsaListTemplateId);
    //console.info("__SafeRunFunctionLoadedScripts :  %o", __SafeRunFunctionLoadedScripts);
    //console.info("_spPageContextInfo : %o", _spPageContextInfo);
    //console.info("ctx : %o", ctx);
    //console.info("WPQ3ListData : %o", WPQ3ListData);
    //console.info("WPQ3SchemaData : %o", WPQ3SchemaData);
    //console.info("_spWebPartComponents : %o", _spWebPartComponents);
    //console.groupEnd();

    //ExecuteOrDelayUntilScriptLoaded(retrieveListItems, "sp.js");

    //$.get('/_api/social.feed/my/news', function (data)
    //{
    //    //console.log(data);
    //});
    //$.get('/_api/social.feed/my/timelinefeed', function (data)
    //{
    //    //console.log(data);
    //});
    //$.get(_spPageContextInfo.siteAbsoluteUrl + "/tr-tr/duyurular/_api/Web/lists/getByTitle('Mikro Akış')/items?$top=10", function (data)
    //{
    //    console.log(data);
    //});
    //$.get(_spPageContextInfo.siteAbsoluteUrl + "/tr-tr/kampanyalar/_api/Web/lists/getByTitle('Mikro Akış')/items?$top=10", function (data)
    //{
    //    console.log(data);
    //});

    innTools.handleIEFixes();

    $('.home-friend-feed #ms-blankfeeddiv a').eq(0).contents().unwrap();

    var orgChart = $('#organizationChartControl');
    if (orgChart.length)
    {
        var intCount = 0;
        var orgInterval = setInterval(function ()
        {
            intCount++;
            var treeNodes = orgChart.find('.treenodediv');
            if (treeNodes.length)
            {
                clearInterval(orgInterval);
                OrgChartInit(treeNodes);
            }

            if (intCount === 4) clearInterval(orgInterval);
        }, 500);
    }

    function OrgChartInit(nodes)
    {
        nodes.each(function (i, item)
        {
            $(item).children('.tnn').on('click', OrgChartSetLink);

            $(item).children('a').on('click', function ()
            {
                var intCount = 0;
                var orgInterval = setInterval(function ()
                {
                    intCount++;
                    var treeNodes = $(item).next('ul').find('.treenodediv');
                    if (treeNodes.length)
                    {
                        clearInterval(orgInterval);
                        OrgChartInit(treeNodes);
                    }

                    if (intCount === 4) clearInterval(orgInterval);
                }, 500);
            });
        });
    }

    function OrgChartSetLink()
    {
        var _link = $(this).find('a').attr('href');
        window.location.href = _link;
    }

    var serverRequestPath = _spPageContextInfo.serverRequestPath;

    if (serverRequestPath.indexOf('/' + innTools.siteNames().Search + '/') > 0 && serverRequestPath.indexOf('summary.aspx') > 0)
    {
        $('table.ms-menutoolbar').last().remove();
    }

    if (serverRequestPath.indexOf('/' + innTools.siteNames().Search + '/') > 0 && serverRequestPath.indexOf('EditForm.aspx') > 0)
    {
        var _ref = _spPageContextInfo.webServerRelativeUrl;
        queryString.push('Source', _ref);
    }

    if (serverRequestPath.indexOf('/' + innTools.siteNames().Search + '/') > 0 && serverRequestPath.indexOf('overview.aspx') > 0)
    {
        var isAdm = innTools.getURLParameterByName('isAdm');

        if (isAdm != 1)
        {
            $('body').hide();
            var _ref = _spPageContextInfo.webServerRelativeUrl;

            STSNavigate(_ref);
        }
    }

    if (serverRequestPath.indexOf('/Forms/') > 0)
    {
        $("a[id*='WebPartMaintenancePageLink']").parent().css('display', 'none');
    }

    if (serverRequestPath.indexOf('/BlogManagement/') > 0 && serverRequestPath.indexOf('NewForm.aspx') > 0)
    {
        $('input[type=text][id*="BlogUrl"]').parent().after('<br/><span class="alert alert-danger">' + RESOURCE.CLIENT_Non_Charecters + '</span><br/><br/>');
        $('input[type=text][id*="BlogUrl"]').bind('keyup', function (e)
        {
            //var index = this.value.indexOf('://'),
            //    length = this.value.length;
            //if (index !== -1)
            //{
            //    this.value = this.value.substring(index + 3, length);
            //}
            this.value = this.value.replace(/[^\w \xC0-\xFF]/g, "");
        });
    }
}

ko.bindingHandlers.dateString = {
    init: function (element, valueAccessor)
    {
        element.onchange = function ()
        {
            var value = valueAccessor();//get our observable
            value(moment(element.value).toDate());
        };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel)
    {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        if (valueUnwrapped)
        {
            var pattern = allBindings.datePattern || 'DD MMM';
            //element.value = moment(valueUnwrapped).format('ll');
            $(element).text(moment(valueUnwrapped).format(pattern));
        }
    }
};
ko.bindingHandlers.scroll = {
    updating: true,
    init: function (element, valueAccessor, allBindingsAccessor)
    {
        var self = this
        self.updating = true;
        var props = allBindingsAccessor().scrollOptions
        ko.utils.domNodeDisposal.addDisposeCallback(element, function ()
        {
            $(props.scrollContainer).off("scroll.ko.scrollHandler");
            self.updating = false;
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor)
    {
        var props = allBindingsAccessor().scrollOptions;
        var offset = props.offset ? props.offset : "0";
        var loadFunc = props.loadFunc;
        var load = ko.utils.unwrapObservable(valueAccessor());
        var self = this;
        if (load)
        {
            element.style.display = "";
            $(props.scrollContainer).on("scroll.ko.scrollHandler", function ()
            {
                if (($(props.scrollContainer).children().last().height() - offset <= $(props.scrollContainer).height() + $(props.scrollContainer).scrollTop()))
                {
                    if (self.updating)
                    {
                        loadFunc();
                        self.updating = false;
                    }
                }
                else
                {
                    self.updating = true;
                }

            });
        }
        else
        {
            //element.style.display = "none";
            $(props.scrollContainer).off("scroll.ko.scrollHandler");
            self.updating = false;
        }
        //console.log($(props.scrollContainer).children().last().height(), ($(props.scrollContainer).children().last().height() - offset <= $(props.scrollContainer).height() + $(props.scrollContainer).scrollTop()));
    }
};
ko.bindingHandlers.trimText = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel)
    {
        var trimmedText = ko.computed(function ()
        {
            var untrimmedText = ko.utils.unwrapObservable(valueAccessor());
            var defaultMaxLength = 20;
            var minLength = 5;
            var maxLength = ko.utils.unwrapObservable(allBindingsAccessor().trimTextLength) || defaultMaxLength;
            if (maxLength < minLength) maxLength = minLength;
            var text = (untrimmedText && untrimmedText.length > maxLength) ? untrimmedText.substring(0, maxLength - 1) + '...' : untrimmedText;
            return text;
        });
        ko.applyBindingsToNode(element, {
            text: trimmedText
        }, viewModel);

        return {
            controlsDescendantBindings: true
        };
    }
};
ko.bindingHandlers.logger = {
    update: function (element, valueAccessor, allBindings)
    {
        //store a counter with this element
        var count = ko.utils.domData.get(element, "_ko_logger") || 0,
            data = ko.toJS(valueAccessor() || allBindings());

        ko.utils.domData.set(element, "_ko_logger", ++count);

        if (window.console && console.log)
        {
            console.log(count, element, data);
        }
    }
};

(function (ko)
{
    // Template used to render the page links
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function (templateName, templateMarkup)
    {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<" + "/script>");
    };

    templateEngine.addTemplate("ko_pager_links", "\
        <div class='pager pagination' data-bind='if: totalPages() > 1'>\
            <ul>\
                <li data-bind='css: {disabled: page() == 1}'><a href='javascript:void(0);' data-bind='click: page.bind($data, 1), enable: page() > 1'>&laquo;</a></li>\
            </ul>\
            <ul data-bind='foreach: relativePages'>\
                <li data-bind='css: { active: $parent.page() == $data }'><a href='javascript:void(0);' data-bind='click: $parent.page.bind($parent, $data), text: $data'></a></span>\
            </ul>\
            <ul>\
                <li data-bind='css: { disabled: page() == totalPages() }'><a href='javascript:void(0);' data-bind='click: page.bind($data, totalPages()), enable: page() < totalPages()'>&raquo;</a></li>\
             </ul>\
       </div>\
    ");

    templateEngine.addTemplate("ko_pager_size", "\
            <select class='pager-size' data-bind='value: itemsPerPage, enable: allowChangePageSize'>\
                <option>10</option>\
                <option>25</option>\
                <option>50</option>\
                <option>100</option>\
            </select>\
    ");

    function makeTemplateValueAccessor(pager)
    {
        return function ()
        {
            return { 'foreach': pager.pagedItems, 'templateEngine': templateEngine };
        };
    }

    function defaultPagerIfEmpty(observable)
    {
        if (observable.pager) return;
        if (ko.isObservable(observable) || !(observable instanceof Function))
            observable.pager = new ko.bindingHandlers.pagedForeach.ClientPager(observable);
        else
            observable.pager = new ko.bindingHandlers.pagedForeach.ServerPager(observable);
    }

    function checkItemPerPageBinding(allBindings, pager)
    {
        if (allBindings['pageSize'])
        {
            pager.itemsPerPage(ko.utils.unwrapObservable(allBindings['pageSize']));

            if (ko.isObservable(allBindings['pageSize']))
            {
                allBindings['pageSize'].subscribe(function (newVal)
                {
                    pager.itemsPerPage(newVal);
                });
                pager.itemsPerPage.subscribe(function (newVal)
                {
                    allBindings['pageSize'](newVal);
                });
            }
        }
    }

    function checkTotalItemsBinding(allBindings, pager)
    {
        if (allBindings['totalItems'] !== undefined && pager.setTotalItems)
        {
            pager.setTotalItems(allBindings['totalItems']);
        }
    }

    ko.bindingHandlers.pagedForeach = {
        Pager: function ()
        {
            var self = this;

            self.page = ko.observable(1);

            self.itemsPerPage = ko.observable(10);
            self.allowChangePageSize = ko.observable(false);

            self.totalItems = ko.observable(0);

            self.totalPages = ko.computed(function ()
            {
                return Math.ceil(self.totalItems() / self.itemsPerPage());
            });

            self.getPageMethod = ko.observable();

            self.pagedItems = ko.computed(function ()
            {
                var itemsPerPage = parseInt(self.itemsPerPage());
                var page = self.page();
                if (self.getPageMethod())
                {
                    return self.getPageMethod()(itemsPerPage, page);
                }
                return [];
            });

            self.relativePages = ko.computed(function ()
            {
                var currentPage = self.page() * 1;
                var totalPages = self.totalPages();
                var pagesFromEnd = totalPages - currentPage;
                var extraPagesAtFront = Math.max(0, 2 - pagesFromEnd);
                var extraPagesAtEnd = Math.max(0, 3 - currentPage);
                var firstPage = Math.max(1, currentPage - (2 + extraPagesAtFront));
                var lastPage = Math.min(self.totalPages(), currentPage + (2 + extraPagesAtEnd));

                return ko.utils.range(firstPage, lastPage);
            });

            self.itemsPerPage.subscribe(function (newVal)
            {
                var n = Math.max(1, Math.ceil(newVal));
                if (n != newVal)
                {
                    self.itemsPerPage(n);
                }
                self.page(1);
            });

            self.page.subscribe(function (newVal)
            {
                var n = (newVal + '').replace(/[^0-9]/g, '');
                var totalPages = self.totalPages();
                if (n < 1)
                {
                    n = 1;
                }
                else if (n > 1 && n > totalPages) n = totalPages;
                if (n != newVal)
                {
                    self.page(n);
                }
            });

            return self;
        },
        ClientPager: function (observableArray, pager)
        {
            if (!pager) pager = new ko.bindingHandlers.pagedForeach.Pager();

            pager.totalItems(ko.utils.unwrapObservable(observableArray).length);

            pager.getPageMethod(function (itemsPerPage, page)
            {
                var array = ko.utils.unwrapObservable(observableArray);
                var indexOfFirstItemOnCurrentPage = ((page - 1) * itemsPerPage);
                var pageArray = array.slice(indexOfFirstItemOnCurrentPage,
                                            indexOfFirstItemOnCurrentPage + itemsPerPage);
                return pageArray;
            });

            if (ko.isObservable(observableArray))
                observableArray.subscribe(function (newArray)
                {
                    pager.totalItems(newArray.length);
                    pager.page(1);
                });

            return pager;
        },
        ServerPager: function (getPageMethod, totalItems, pager)
        {
            if (!pager) pager = new ko.bindingHandlers.pagedForeach.Pager();

            pager.getPageMethod(getPageMethod);

            pager.setTotalItems = function (totItems)
            {

                pager.totalItems(ko.utils.unwrapObservable(totItems));

                if (ko.isObservable(totItems))
                    totItems.subscribe(function (newCount)
                    {
                        pager.totalItems(newCount);
                    });
            };

            if (totalItems) pager.setTotalItems(totalItems);

            return pager;
        },
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor(), allBindings = allBindingsAccessor();
            defaultPagerIfEmpty(observable);
            checkItemPerPageBinding(allBindings, observable.pager);
            checkTotalItemsBinding(allBindings, observable.pager);
            if (ko.isObservable(observable))
                var array = ko.utils.unwrapObservable(observable);
            return ko.bindingHandlers.template.init(element, makeTemplateValueAccessor(observable.pager));
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor();
            if (ko.isObservable(observable))
                var array = ko.utils.unwrapObservable(observable);
            return ko.bindingHandlers.template.update(element, makeTemplateValueAccessor(observable.pager), allBindingsAccessor, viewModel, bindingContext);
        }
    };

    ko.bindingHandlers.pageSizeControl = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor(), allBindings = allBindingsAccessor();
            defaultPagerIfEmpty(observable);
            checkItemPerPageBinding(allBindings, observable.pager);
            checkTotalItemsBinding(allBindings, observable.pager);
            return { 'controlsDescendantBindings': true };
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor();
            var array = ko.utils.unwrapObservable(observable);
            defaultPagerIfEmpty(observable);

            observable.pager.allowChangePageSize(true);

            // Empty the element
            while (element.firstChild) ko.removeNode(element.firstChild);

            // Render the page links
            ko.renderTemplate('ko_pager_size', observable.pager, { templateEngine: templateEngine }, element);
        }
    };

    ko.bindingHandlers.pageLinks = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor(), allBindings = allBindingsAccessor();
            defaultPagerIfEmpty(observable);
            checkItemPerPageBinding(allBindings, observable.pager);
            checkTotalItemsBinding(allBindings, observable.pager);
            return { 'controlsDescendantBindings': true };
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
        {
            var observable = valueAccessor();
            var array = ko.utils.unwrapObservable(observable);
            defaultPagerIfEmpty(observable);

            // Empty the element
            while (element.firstChild) ko.removeNode(element.firstChild);

            // Render the page links
            ko.renderTemplate('ko_pager_links', observable.pager, { templateEngine: templateEngine }, element, "replaceNode");
        }
    };
}(ko));

$(innTools.pageInit);

function retrieveListItems()
{
    var clientContext = new SP.ClientContext(_spPageContextInfo.siteAbsoluteUrl);
    var oList = clientContext.get_web().get_lists().getByTitle('Etkinlikler');

    var camlQuery = new SP.CamlQuery();
    var _camlXml =
        "<View> <ViewFields> <FieldRef Name='Title' /> <FieldRef Name='StartDate' /> <FieldRef Name='EndDate' /> </ViewFields> <OrderBy> <FieldRef Name='EndDate' /> </OrderBy> <Query> <Where> <And> <Leq> <FieldRef Name='StartDate' /> <Value Type='DateTime'> <Today /> </Value> </Leq> <Geq> <FieldRef Name='EndDate' /> <Value Type='DateTime'> <Today /> </Value> </Geq> </And> </Where></Query></View>";

    camlQuery.set_viewXml(_camlXml);
    this.collListItem = oList.getItems(camlQuery);
    clientContext.load(collListItem);
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
    function onQuerySucceeded(sender, args)
    {
        var listItemInfo = '';
        var listItemEnumerator = collListItem.getEnumerator();
        while (listItemEnumerator.moveNext())
        {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo += String.format('\nID: {0}\nTitle:{1}', oListItem.get_id(), oListItem.get_item('Title'));
        }
    }

    function onQueryFailed(sender, args)
    {
        console.log('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }

}

//Data Models
function NewsItemModel(data)
{
    this.ArticleDate = ko.observable(data.ArticleDate);
    this.Author = ko.observable(data.Author);
    this.AuthorLoginName = ko.observable(data.AuthorLoginName);
    this.Body = ko.observable(data.Body);
    this.Category = ko.observable(data.Category);
    this.City = ko.observable(data.City);
    this.EndDate = ko.observable(data.EndDate);
    this.EventId = ko.observable(data.EventId);
    this.ID = ko.observable(data.ID);
    this.ImageUrl = ko.observable(data.ImageUrl || '/_layouts/15/xPortal/Assets/img/x_250x250.jpg');
    this.LikeCount = ko.observable(data.LikeCount);
    this.MovieId = ko.observable(data.MovieId);
    this.PlaceId = ko.observable(data.PlaceId);
    this.PublishDate = ko.observable(data.PublishDate);
    this.SalesUrl = ko.observable((data.SalesUrl == null) ? false : data.SalesUrl.split(',')[0]);
    this.StartDate = ko.observable(data.StartDate);
    this.Title = ko.observable(data.Title);
    this.Url = ko.observable(data.Url);

    return this;
}

function EventItemModel(data)
{
    this.Attendees = ko.observable(data.Attendees);
    this.Capacity = ko.observable(data.Capacity);
    this.Description = ko.observable(data.Description);
    this.EventTime = ko.observable(data.EventTime);
    this.EventId = ko.observable(data.Id);
    this.Location = ko.observable(data.Location);
    this.ShowAttendees = ko.observable(data.ShowAttendees);
    this.Title = ko.observable(data.Title);
}

function FeedItemModel(data)
{
    return {
        ActorName: ko.observable(data.ActorName),
        Created: ko.observable(data.Created),
        Permalink: ko.observable(data.Permalink),
        ProfileImageUrl: ko.observable(data.ProfileImageUrl ? data.ProfileImageUrl : "/_layouts/images/o14_person_placeholder_32.png"),
        Text: ko.observable(data.Text)
    };
}

function SocialFeedModel(data)
{
    return {
        Category: ko.observable(data.Category),
        PostDate: ko.observable(moment(data.PostDate).format('L')),
        PostUrl: ko.observable(data.PostUrl),
        Text: ko.observable(data.Text)
    };
}

function CompanyItemModel(data)
{
    var _logo = function (img)
    {
        return String.format('{0}?RenditionId=17', $(img).attr('src'));
    };
    return {
        "Address": data.Address,
        "AuthorizedUser": data.AuthorizedUser,
        "AverageRating": data.AverageRating,
        "City": data.City,
        "CompanyName": data.CompanyName,
        "ContentType": data.ContentType,
        "Country": data.Country,
        "FaxNumber": data.FaxNumber,
        "GenericDescription": data.GenericDescription,
        "Id": data.Id,
        "IsDefault": data.IsDefault,
        "Logo": _logo(data.Logo),
        "PhoneNumber": data.PhoneNumber,
        "RatingCount": data.RatingCount,
        "Region": data.Region,
        "Sector": data.Sector,
        "SiteUrl": data.SiteUrl,
        "Title": data.Title,
        "Url": data.Url,
        "WebAddress": data.WebAddress
    };
}

function AnnouncementItemModel(data)
{
    this.Created = ko.observable(data.Created || "");
    this.Description = ko.observable(data.Description || "");
    this.Id = ko.observable(data.Id || "");
    this.ImageUrl = ko.observable(data.ImageUrl || "");
    this.LikeCount = ko.observable(data.LikeCount || "");
    this.OrderNumber = ko.observable(data.OrderNumber || "");
    this.Title = ko.observable(data.Title || "");
    this.Url = ko.observable(data.Url || "");
    return this;
}

function CompanyDetailModel(data)
{
    return {
        "Title": data ? data.Title : "",
        "Address": data ? data.Address : "",
        "AuthorizedUser": data ? data.AuthorizedUser : "",
        "FaxNumber": data ? data.FaxNumber : "",
        "PhoneNumber": data ? data.PhoneNumber : "",
        "County": data ? data.County : "",
        "WebAddress": data ? data.WebAddress : ""
    };
}
//View Models
var menuViewModel = function ()
{
    var self = this;
    self.menuItems = ko.observableArray();
    $.get(String.format("/{0}{1}/{2}", _spPageContextInfo.currentCultureName, sa_config.portalBaseServiceUrl, 'GetMenuItems'), function (data)
    {
        ko.mapping.fromJS(data.Data.ShortcutItems, {}, self.menuItems);
    });

};

function TweeterViewModel()
{
    var self = this,
        lastDate = new Date(),
        itemCount = 5;
    self.tweets = ko.observableArray([]);
    self.errorText = ko.observable(false);
    self.isLastItem = ko.observable(true);

    this.render = function ()
    {
        $.ajax({
            type: 'POST',
            url: sa_config.portalBaseServiceUrl + '/GetLatestActiveTwitterFeed',
            data: innTools.JSONSerializer({ NewestDate: lastDate, ItemCount: itemCount }),
            success: function (data, textStatus, jqXHR)
            {
                var _lastItem = data.Data.slice(-1);
                lastDate = (_lastItem.length > 0) ? _lastItem[0].TweetDate : null;
                self.isLastItem((lastDate == null) ? false : true);
                $.each(data.Data, function (i, item) { self.tweets.push(item); });

                //lastDate = (data.Data.length > 0 && data.Data.slice(-1) != null) ? data.Data.slice(-1)[0].TweetDate : null;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };
    this.render();

    $('#streamContent').ClassyScroll();
}

var exchangeViewModel = function ()
{
    var self = this;
    self.exchange = ko.observableArray([]);

    $.get(sa_config.portalBaseServiceUrl + '/GetLatestExchangeRates', function (data, textStatus, jqXHR)
    {
        ko.mapping.fromJS(data.Data, {}, self.exchange);
    });
};

var weatherViewModel = function ()
{
    var _url = $('#userInfoPart a.name').attr('href');
    var self = this,
        _loginName = innTools.getURLParameterByName('accountname', _url),
        _cityCookies = $.cookie('saport_weather_city'),
        _city = arguments[0];
    self.City = ko.observable({});

    self.WeatherInfo = ko.observable({});
    self.ResultStatus = ko.observable(false);

    self.getLocation = function ()
    {
        $.post(sa_config.portalBaseServiceUrl + '/GetLocation', innTools.JSONSerializer({ LoginName: _loginName, CityName: '' }), function (data, textStatus, jqXHR)
        {
            if (data.Data.CityName || data.Data.CityName != "")
            {
                _city = data.Data.CityName;
            }

            self.City(_city);
            self.render();
        });
    };

    self.update = function ()
    {
        _city = arguments[0] || _city;
        $.cookie('saport_weather_city', _city);
        $.post(sa_config.portalBaseServiceUrl + '/SetUserLocation', innTools.JSONSerializer({ LoginName: _loginName, CityName: _city }), function (data, textStatus, jqXHR)
        {
            self.render();
        });
    };

    self.render = function ()
    {
        $.post(sa_config.portalBaseServiceUrl + '/GetCityWeatherStatus', innTools.JSONSerializer({ "CityName": _city }), function (data, textStatus, jqXHR)
        {
            if (data.Data != null && data.Status.Code == "0")
            {

                self.City(_city.replace(' - Sabancı Center', ''));
                self.WeatherInfo(data.Data.WeatherInfo);
                self.ResultStatus(true);
            }
            else
            {
                self.ResultStatus(false);
            }
        });
    };

    self.getLocation();
};

function EventViewModel()
{
    var self = this,
        calendarControl = arguments[0] || "#calendarControl";
    self.eventList = ko.observableArray([]);
    self.ResultStatus = ko.observable(false);

    var calendarPicker2 = $(calendarControl).calendarPicker({
        monthNames: arguments[1] || RESOURCE.CLIENT_Mouth_Names,
        dayNames: arguments[2] || RESOURCE.CLIENT_Calendar_Day_Short_Names,
        navText: { prevText: "<i class='icon-left-dir'> " + RESOURCE.CLIENT_Event_Prev_Text, nextText: RESOURCE.CLIENT_Event_Next_Text + " <i class='icon-right-dir'>" },
        years: 0,
        months: 0,
        days: 4,
        showDayArrows: true,
        callback: function (cal)
        {
            self.update(cal.currentDate);
        }
    });

    this.update = function ()
    {
        var _date = arguments[0] || new Date();
        $.post("/" + _spPageContextInfo.currentCultureName + "/" + sa_config.portalBaseServiceUrl + '/GetEventList', innTools.JSONSerializer({ IncludeDate: _date, IsWeekly: true, Count: 10 }), function (data, textStatus, jqXHR)
        {
            self.eventList.removeAll();

            ko.utils.arrayMap(data.Data, function (item)
            {
                self.eventList.push(item);
            });
            if (data.Data != null && data.Status.Code == "0")
            {
                self.ResultStatus(true);
            }
            else
            {
                self.ResultStatus(false);
            }
        }).fail(function ()
        {
            self.eventList.removeAll();
            self.ResultStatus(false);
        });
    };
    this.update();


    $('#EventsControl .scrolled').ClassyScroll();
}

function EventDetailViewModel()
{
    var self = this;
    self.status = ko.observable(false),
    self.statusText = ko.observable();
    self.AttendeeStatus = ko.observable(true);
    self.buttonVisible = ko.observable(true);
    self.AttendeeList = ko.observableArray([]);

    this.IsUserAttendRequest = function ()
    {
        var _eventId = parseInt(innTools.getURLParameterByName('EventId'));

        $.post("/" + _spPageContextInfo.currentCultureName + "/" + sa_config.portalBaseServiceUrl + '/IsUserAttendRequest', innTools.JSONSerializer({ IsAttendRequest: false, EventId: _eventId }),
            function (data, textStatus, jqXHR)
            {
                if (data.Status != null)
                {
                    switch (data.Status.Code)
                    {
                        case 2:
                            self.buttonVisible(false);
                            break;
                        case 3:
                            self.buttonVisible(false);
                            break;
                        case 4:
                            self.buttonVisible(false);
                            break;
                        case 5:
                            self.AttendeeStatus(false);
                            self.buttonVisible(true);
                            break;
                        case 6:
                            self.AttendeeStatus(true);
                            self.buttonVisible(true);
                            break;
                        case 7:
                            self.buttonVisible(false);
                            break;
                        default:
                    }

                    self.statusText(data.Status.Message);
                    if (data.Status.Message == null)
                    {
                        self.status(false);
                    } else
                    {
                        self.status(true);
                    }
                } else
                {
                    self.status(false);
                }
            });
    };

    this.IsUserAttendRequest();

    self.Attendee = function (e, event)
    {
        var _elem = $(event.currentTarget),
            _request = self.AttendeeStatus(),
            _eventId = parseInt(innTools.getURLParameterByName('EventId'));

        self.status(false);

        $.post("/" + _spPageContextInfo.currentCultureName + "/" + sa_config.portalBaseServiceUrl + '/AttendEventInfo', innTools.JSONSerializer({ IsAttendRequest: _request, EventId: _eventId }), function (data, textStatus, jqXHR)
        {
            if (data.Status != null)
            {
                var _attendeeList = jQuery.parseJSON(data.Data.AttendeeList);

                self.AttendeeList(_attendeeList);

                self.statusText(data.Status.Message);
                self.AttendeeStatus(!self.AttendeeStatus());

                if (data.Status.Message == null)
                {
                    self.status(false);
                } else
                {
                    self.status(true);
                }
            } else
            {
                self.status(false);
            }



        }).fail(function ()
        {
            self.status(false);
        });
    };
}

function MealViewModel()
{
    var self = this,
        dayValue = -1;
    self.mealList = ko.observableArray([]);

    self.render = function ()
    {
        $.post(sa_config.portalBaseServiceUrl + '/GetMealOfDay', innTools.JSONSerializer({ Day: dayValue }),
            function (data, textStatus, jqXHR)
            {
                if (data.Data != null)
                {
                    ko.mapping.fromJS(data.Data.MealContents, {}, self.mealList);
                }
            });
    };

    //self.changeDay = function (e, event) {
    //    var _currentSelect = $(event.currentTarget);
    //    dayValue = (_currentSelect.val()) ? _currentSelect.val() : '';
    //    self.mealList.removeAll();
    //    self.render();
    //};

    self.changeDay = function (val)
    {
        dayValue = (val) ? val : '';
        self.mealList.removeAll();
        self.render();
    };

    self.render();

}

function LatestNewsViewModel()
{
    var self = this,
        newsSite = arguments[0] || '/' + _spPageContextInfo.currentCultureName + '/' + innTools.siteNames().News,
        maxItems = arguments[1] || 3,
        orderType = arguments[2] || '',
        itemIndex = 0;
    self.newsList = ko.observableArray([]);
    self.update = function ()
    {
        $.post(newsSite + sa_config.portalBaseServiceUrl + '/GetLatestNews', innTools.JSONSerializer({ NewestDate: new Date(), ItemCount: maxItems, Filter: { 'OrderType': orderType }, Index: itemIndex }), function (data, textStatus, jqXHR)
        {
            if (data.Data != null)
            {
                itemIndex = itemIndex + maxItems;
                ko.mapping.fromJS(data.Data, {}, self.newsList);
            }
        });
    };
    self.update();

    //$('#News_ArtNCultural_Control ul').ClassyScroll();
}

function LatestCultereNewsModel()
{
    var self = this,
        lastDate = new Date(),
        _params = arguments[0],
        itemCount = 5,
        itemIndex = 0,
        _preloading = false,
        _ajaxData = {};
    self.CultureNewsList = ko.observableArray([]);
    self.errorText = ko.observable(false);
    self.isLastItem = ko.observable(true);
    self.loadingPanel = ko.observable(false);
    self.emptyPanel = ko.observable(false);

    _ajaxData["Filter"] = {};
    _ajaxData["Filter"]["OrderType"] = _params.orderType;
    _ajaxData["Filter"]["FirstResponse"] = true;

    this.render = function ()
    {
        if (_preloading) return false;

        _ajaxData["NewestDate"] = lastDate;
        _ajaxData["ItemCount"] = itemCount;
        _ajaxData["Index"] = itemIndex;

        self.loadingPanel(true);
        self.emptyPanel(false);
        _preloading = true;

        $.ajax({
            type: 'POST',
            url: '/' + _spPageContextInfo.currentCultureName + '/kulturvesanat' + sa_config.portalBaseServiceUrl + '/GetLatestNews',
            data: innTools.JSONSerializer(_ajaxData),
            success: function (data, textStatus, jqXHR)
            {
                var _lastItem = data.Data.slice(-1);
                lastDate = (_lastItem.length > 0) ? _lastItem[0].PublishDate : null;
                self.isLastItem((lastDate == null) ? false : true);
                itemIndex = itemIndex + itemCount;
                $.each(data.Data, function (i, item)
                {
                    item.ImageUrl = item.ImageUrl || '/_layouts/15/xPortal/Assets/img/culture_default.jpg';
                    self.CultureNewsList.push(new NewsItemModel(item));
                });

                self.loadingPanel(false);

                if (self.CultureNewsList().length > 0)
                {
                    self.emptyPanel(false);
                } else
                {
                    self.emptyPanel(true);
                }

                if (_ajaxData["Filter"].FirstResponse)
                {
                    _ajaxData["Filter"].FirstResponse = false;
                }

                _preloading = false;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                _preloading = false;
                self.errorText(true);
            }
        });
    };

    self.preRenderForm = function ()
    {
        self.CultureNewsList.removeAll();
        itemIndex = 0;
        lastDate = new Date();

        $([_params.filterElems.Category, _params.filterElems.City]).each(function (e, item)
        {
            var _currentSelect = $(this);
            var _param = _currentSelect.data('name');
            _ajaxData["Filter"][_param] = (_currentSelect.val() === "All") ? "" : _currentSelect.val();
        });
    };

    self.changeDate = function (e, a)
    {
        self.CultureNewsList.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        _ajaxData["Filter"]["Date"] = e.date;
        self.render();
    };

    this.selectionChanged = function (e, event)
    {
        self.CultureNewsList.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        var _currentSelect = $(event.currentTarget);
        var _param = _currentSelect.data('name');
        _ajaxData["Filter"][_param] = (_currentSelect.val() === "All") ? "" : _currentSelect.val();
        self.render();
    };

    self.clearDateFilter = function (e)
    {
        e.date = "";
        self.CultureNewsList.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        $(_params.filterElems.Date).val('');
        _ajaxData.Filter = self.deleteObjFromObj(_ajaxData.Filter, "Date");
        _preloading = false;
        self.render();
        setTimeout(function ()
        {
            $(_params.filterElems.Date).blur();
        }, 100);
    };

    self.deleteObjFromObj = function (obj, deleteItem)
    {
        var objKeys = Object.keys(obj);
        var newObj = new Object();
        for (var i = 0, objLength = objKeys.length; i < objLength; i++)
        {
            if (objKeys[i] != deleteItem)
            {
                newObj[objKeys[i]] = obj[objKeys[i]];
            }
        }
        return newObj;
    }

    self.bindQTip = function (model, event)
    {
        $(event.currentTarget).qtip({
            overwrite: false,
            content: {
                button: true,
                text: function (event, api)
                {
                    return $(this).find('.tooltip-content').html();
                },
                title: RESOURCE.CLIENT_Event_Tooltip_Title
            },
            show: {
                event: event.type,
                ready: true
            },
            hide: {
                fixed: true,
                delay: 300
            },
            position: {
                my: 'center left',
                at: 'center right'
            },
            style: {
                classes: 'qtip-bootstrap'
            }
        });
    };

    self.allCANews = function (model, event)
    {
        var _allLink = $(event.currentTarget).attr('href');

        var _status = _ajaxData.Filter.Category != "" || _ajaxData.Filter.City != "" || (_ajaxData.Filter.Date != undefined && _ajaxData.Filter.Date != "");
        if (_status)
        {
            _allLink += String.format("?Category={0}&City={1}&Date={2}",
                                                                            encodeURIComponent(_ajaxData.Filter.Category),
                                                                            encodeURIComponent(_ajaxData.Filter.City),
                                                                            encodeURIComponent(innTools.JSONSerializer(_ajaxData.Filter.Date)));
        }
        window.location = _allLink;
        return false;
    };

    _params.filterElems.Date.watermark();
    _params.filterElems.Date.datepicker({
        format: "dd.mm.yyyy",
        language: innTools.siteNames().id,
        autoclose: true,
        clearBtn: true,
        todayBtn: 'linked',
        todayHighlight: true
    })
    .on("changeDate", self.changeDate)
    .on("clearDate", self.clearDateFilter);

    this.preRenderForm();
    this.render();
}

function AllNewsModel()
{
    var self = this,
        lastDate = new Date(),
        _params = arguments[0],
        itemCount = 5,
        itemIndex = 0,
        _preloading = false,
        _ajaxData = {};
    self.newsItems = ko.observableArray([]);
    self.errorText = ko.observable(false);
    self.isLastItem = ko.observable(true);
    self.loadingPanel = ko.observable(false);

    self.emptyPanel = ko.observable(false);
    self.CategoryValue = ko.observable("");
    self.CityValue = ko.observable("");
    self.DateValue = ko.observable("");
    self.RateOrder = ko.observable(false);

    _ajaxData["Filter"] = {};
    _ajaxData["Filter"]["OrderType"] = _params.orderType;
    _ajaxData["Filter"]["FirstResponse"] = true;
    _ajaxData["Filter"]["RateOrder"] = self.RateOrder();

    self.render = function ()
    {
        self.loadingPanel(true);
        if (_preloading) return false;

        _ajaxData["NewestDate"] = lastDate;
        _ajaxData["ItemCount"] = itemCount;
        _ajaxData["Index"] = itemIndex;
        _ajaxData["Filter"]["RateOrder"] = self.RateOrder();

        self.loadingPanel(true);

        self.emptyPanel(false);

        _preloading = true;

        $.ajax({
            type: 'POST',
            url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetLatestNews',
            data: innTools.JSONSerializer(_ajaxData),
            beforeSend: function ()
            {

            },
            success: function (data, textStatus, jqXHR)
            {
                var _lastItem = data.Data.slice(-1);
                lastDate = (_lastItem.length > 0) ? _lastItem[0].PublishDate : null;
                self.isLastItem((lastDate == null) ? false : true);
                itemIndex = itemIndex + itemCount;
                $.each(data.Data, function (i, item)
                {
                    if (_params.newsType == 'Culture')
                    {
                        item.ImageUrl = item.ImageUrl || '/_layouts/15/xPortal/Assets/img/culture_default.jpg';
                    }
                    self.newsItems.push(new NewsItemModel(item));
                });


                self.loadingPanel(false);

                if (self.newsItems().length > 0)
                {
                    self.emptyPanel(false);
                } else
                {
                    self.emptyPanel(true);
                }

                if (_ajaxData["Filter"].FirstResponse)
                {
                    _ajaxData["Filter"].FirstResponse = false;
                }

                _preloading = false;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };

    self.selectionChanged = function (e, event)
    {
        self.newsItems.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        var _currentSelect = $(event.currentTarget);
        var _param = _currentSelect.data('name');
        _ajaxData["Filter"][_param] = (_currentSelect.val() === "All") ? "" : _currentSelect.val();
        self.render();
    };

    self.changeDate = function (e)
    {
        self.newsItems.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        _ajaxData["Filter"]["Date"] = e.date;
        self.render();
    };

    self.clearDateFilter = function (e)
    {
        self.newsItems.removeAll();
        itemIndex = 0;
        lastDate = new Date();
        _ajaxData["Filter"]["Date"] = null;
        _params.filterElems.Date.val('').blur();
        self.render();
    };

    self.preRenderForm = function ()
    {
        self.newsItems.removeAll();
        itemIndex = 0;
        lastDate = new Date();

        var queryUri = {
            Category: decodeURIComponent(innTools.getURLParameterByName('Category')),
            City: decodeURIComponent(innTools.getURLParameterByName('City')),
            Date: decodeURIComponent(innTools.getURLParameterByName('Date'))
        };

        if (queryUri.Category)
        {
            _ajaxData["Filter"]["Category"] = queryUri.Category;
            self.CategoryValue(queryUri.Category);
            _params.filterElems.Category.selectpicker('val', queryUri.Category).change();
            _params.filterElems.Category.selectpicker('refresh');
        }
        if (queryUri.City)
        {
            _ajaxData["Filter"]["City"] = queryUri.City;
            self.CityValue(_ajaxData.Filter.City);
            _params.filterElems.City.selectpicker('val', _ajaxData.Filter.City).change();
            _params.filterElems.City.selectpicker('refresh');
        }
        if (queryUri.Date && queryUri.Date != "null")
        {
            _ajaxData["Filter"]["Date"] = new Date(innTools.JSONDeserializer(queryUri.Date));
            self.DateValue(_ajaxData.Filter.Date);
            _params.filterElems.Date.datepicker('update', _ajaxData.Filter.Date);
        }

        $([_params.filterElems.Category, _params.filterElems.City]).each(function (e, item)
        {
            var _currentSelect = $(this);
            var _param = _currentSelect.data('name');
            _ajaxData["Filter"][_param] = (_currentSelect.val() === "All") ? "" : _currentSelect.val();
        });
    };

    self.onRateStatus = function (model, event)
    {
        self.newsItems.removeAll();

        var _rate = self.RateOrder();

        self.RateOrder(!_rate);

        lastDate = new Date();
        itemCount = 5;
        itemIndex = 0;

        self.render();
    };

    _params.filterElems.Date.watermark();
    _params.filterElems.Date.datepicker({
        format: "dd.mm.yyyy",
        language: innTools.siteNames().id,
        autoclose: true
    }).on("changeDate", self.changeDate);

    this.preRenderForm();
    this.render();
}

function BlogModel()
{
    var self = this,
        lastDate = new Date(),
        itemCount = 3;
    self.blogItems = ko.observableArray([]);
    self.errorText = ko.observable(false);

    this.render = function ()
    {
        $.ajax({
            type: 'POST',
            url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetLatestBlogPosts',
            data: innTools.JSONSerializer({ LastPostDate: lastDate, ItemCount: itemCount }),
            success: function (data, textStatus, jqXHR)
            {
                lastDate = (data.Data.length > 0 && data.Data.slice(-1) != null) ? data.Data.slice(-1)[0].PublishedDate : null;
                $.each(data.Data, function (i, item) { self.blogItems.push(item); });
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };
    this.render();
}

function AllPortalFeedsModel()
{
    var self = this,
        lastDate = new Date(),
        itemCount = 20,
        itemIndex = 0,
        filter = 'All';
    _preloading = false,
    args = arguments[0],
    _ajaxData = {};
    self.feedItems = ko.observableArray([]);
    self.errorText = ko.observable(false);
    self.isLastItem = ko.observable(true);
    self.categories = ko.observableArray([
        {
            Name: RESOURCE.CLIENT_All_Text, Type: 'All'
        },
        {
            Name: RESOURCE.CLIENT_Announcements_Text, Type: 'Announcements'
        },
        {
            Name: RESOURCE.CLIENT_News_Text, Type: 'News'
        }

    ]); //All", "Announcements", "News", "Documents", "Campaigns"

    if (innTools.siteNames().id == "tr")
    {
        self.categories.push({
            Name: RESOURCE.CLIENT_Documents_Text, Type: 'Documents'
        });
        self.categories.push(
        {
            Name: RESOURCE.CLIENT_Campaigns_Text, Type: 'Campaigns'
        });
    }

    self.loadingPanel = ko.observable(false);
    self.emptyPanel = ko.observable(false);
    self.FeedTitle = ko.observable(RESOURCE.CLIENT_All_Text);

    self.render = function ()
    {
        self.loadingPanel(true);
        if (_preloading) return false;

        _ajaxData["NewestDate"] = lastDate;
        _ajaxData["ItemCount"] = itemCount;
        _ajaxData["Index"] = itemIndex;
        _ajaxData["Filter"] = filter;

        self.loadingPanel(true);

        self.emptyPanel(false);

        _preloading = true;

        $.ajax({
            type: 'POST',
            url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetSocialFeeds',
            data: innTools.JSONSerializer(_ajaxData),
            success: function (data, textStatus, jqXHR)
            {
                args.ddlCategories.selectpicker('refresh');

                var _lastItem = data.Data.slice(-1);
                lastDate = (_lastItem.length > 0) ? _lastItem[0].PostDate : null;
                self.isLastItem((lastDate == null) ? false : true);
                itemIndex = itemIndex + itemCount;
                $.each(data.Data, function (i, item)
                {
                    self.feedItems.push(new SocialFeedModel(item));
                });

                self.loadingPanel(false);

                if (self.feedItems().length > 0)
                {
                    self.emptyPanel(false);
                } else
                {
                    self.emptyPanel(true);
                }

                _preloading = false;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };

    self.ddlChange = function (model, event)
    {
        self.feedItems.removeAll();

        var _value = event.currentTarget.value,
            _selectedText = $(event.currentTarget.selectedOptions).text();

        self.FeedTitle(_selectedText);

        filter = _value;
        itemIndex = 0;
        lastDate = new Date();

        self.render();
    };

    self.render();
}

function CommunityCompaniesController()
{
    var self = this,
        _preloading = false,
        args = arguments[0],
        _ajaxData = { Region: '', Country: '', City: '', CompanyName: '' };

    self.RegionNames = ko.observableArray([]);
    self.CountryNames = ko.observableArray([]);
    self.CityNames = ko.observableArray([]);
    self.CompanyNames = ko.observableArray([]);

    self.CompanyItems = ko.observableArray([]);
    self.errorText = ko.observable(false);

    self.loadingPanel = ko.observable(false);
    self.emptyPanel = ko.observable(false);

    self.pageSize = ko.observable(10);

    self.render = function ()
    {
        self.loadingPanel(true);

        if (_preloading) return false;

        self.emptyPanel(false);
        _preloading = true;
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetCommunityCompaniesList',
            data: innTools.JSONSerializer(_ajaxData),
            success: function (data, textStatus, jqXHR)
            {
                self.CompanyItems.removeAll();

                if (data.Data.Regions !== null && self.RegionNames().length < 1)
                {
                    $.each(data.Data.Regions, function (i, item)
                    {
                        self.RegionNames.push(item);
                    });
                }

                if (data.Data.Countries !== null && self.CountryNames().length < 1)
                {
                    $.each(data.Data.Countries, function (i, item)
                    {
                        self.CountryNames.push(item);
                    });
                }

                if (data.Data.Cities !== null && self.CityNames().length < 1)
                {
                    $.each(data.Data.Cities, function (i, item)
                    {
                        self.CityNames.push(item);
                    });
                }

                if (data.Data.Companies !== null && self.CompanyNames().length < 1)
                {
                    $.each(data.Data.Companies, function (i, item)
                    {
                        self.CompanyNames.push(item);
                    });
                }

                args.ddlRegionNames.selectpicker('refresh');
                args.ddlCountryNames.selectpicker('refresh');
                args.ddlCityNames.selectpicker('refresh');
                args.ddlCompanyNames.selectpicker('refresh');

                $.each(data.Data.CommunityCompaniesList, function (i, item)
                {
                    self.CompanyItems.push(new CompanyItemModel(item));
                });



                self.loadingPanel(false);

                if (self.CompanyItems().length > 0)
                {
                    self.emptyPanel(false);
                } else
                {
                    self.emptyPanel(true);
                }

                _preloading = false;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };

    self.ddlChange = function (model, event)
    {
        var _type = $(event.currentTarget).data('type'),
            _value = event.currentTarget.value;
        switch (_type)
        {
            case 'Region':
                _ajaxData.Region = _value;

                _ajaxData.Country = "";
                _ajaxData.City = "";
                _ajaxData.CompanyName = "";

                self.CountryNames.removeAll();
                self.CityNames.removeAll();
                self.CompanyNames.removeAll();
                break;
            case 'Country':
                _ajaxData.Country = _value;

                _ajaxData.City = "";
                _ajaxData.CompanyName = "";

                self.CityNames.removeAll();
                self.CompanyNames.removeAll();
                break;
            case 'City':
                _ajaxData.City = _value;

                _ajaxData.CompanyName = "";

                self.CompanyNames.removeAll();
                break;
            case 'CompanyName':
                _ajaxData.CompanyName = _value;
                break;

        }

        self.render();
    };
    self.render();

}

function SiteFollowModel()
{
    var self = this,
        socialURI = _spPageContextInfo.webAbsoluteUrl + '/_api/social.following',
        _spIsFollowedModel = {
            "actor": {
                "__metadata": { "type": "SP.Social.SocialActorInfo" },
                "ActorType": 2,
                "ContentUri": _spPageContextInfo.webAbsoluteUrl,
                "Id": null
            }
        };
    self.isFollowed = ko.observable(false);

    $.post(socialURI + '/isfollowed', innTools.JSONSerializer(_spIsFollowedModel), function (data, textStatus, jqXHR)
    {
        self.isFollowed(data.d.IsFollowed);
    });

    self.AddFollow = function ()
    {
        if (!self.isFollowed())
        {
            $.post(socialURI + '/follow', innTools.JSONSerializer(_spIsFollowedModel), function (data, textStatus, jqXHR)
            {
                self.isFollowed(true);
            });
        }
        return false;
    };

    self.removeFollow = function ()
    {
        $.post(socialURI + '/stopfollowing', innTools.JSONSerializer(_spIsFollowedModel), function (data, textStatus, jqXHR)
        {
            self.isFollowed(false);
        });
        return false;
    };
}

function PeopleFollowModel()
{
    var self = this,
        socialURI = _spPageContextInfo.webAbsoluteUrl + '/_api/social.following',
        _followUrl = "{0}/_api/social.following/{1}(ActorType=0,AccountName=@v,Id=null)?@v='{2}'",
        _accountName = innTools.getURLParameterByName('accountname'),
        _spIsFollowedModel = {
            "actor": {
                "__metadata": { "type": "SP.Social.SocialActorInfo" },
                "ActorType": 0,
                "AccountName": _accountName,
                "Id": null
            }
        };

    self.isFollowed = ko.observable(false);
    self.isVisible = ko.observable(false);

    if (_accountName == "")
    {
        self.isVisible(false);
    } else
    {
        self.isVisible(true);
        $.post(socialURI + '/isfollowed', innTools.JSONSerializer(_spIsFollowedModel), function (data, textStatus, jqXHR)
        {
            self.isFollowed(data.d.IsFollowed);
        });
    }

    self.AddFollow = function ()
    {
        if (!self.isFollowed())
        {
            $.post(String.format(_followUrl, _spPageContextInfo.webAbsoluteUrl, 'follow', _accountName), function (data, textStatus, jqXHR)
            {
                self.isFollowed(true);
            });
        }
        return false;
    };

    self.removeFollow = function ()
    {
        $.post(String.format(_followUrl, _spPageContextInfo.webAbsoluteUrl, 'stopfollowing', _accountName), function (data, textStatus, jqXHR)
        {
            self.isFollowed(false);
        });
        return false;
    };
}

function GetFeedsModel()
{
    var self = this;
    self.feedItems = ko.observableArray([]);
    self.ResultStatus = ko.observable(false);
    self.LoadingStatus = ko.observable(false);

    self.render = function ()
    {
        self.LoadingStatus(true);
        $.post(sa_config.portalBaseServiceUrl + '/GetFeeds', innTools.JSONSerializer({ FriendsOnly: true }), function (data, textStatus, jqXHR)
        {
            if (data.Data != null)
            {
                //ko.mapping.fromJS(data.Data, {}, self.feedItems);
                var _index = 0;
                ko.utils.arrayMap(data.Data, function (item)
                {
                    _index++;
                    if (_index < 11)
                    {
                        self.feedItems.push(new FeedItemModel(item));
                    }
                });
                self.ResultStatus(true);
            }
            else
            {
                self.ResultStatus(false);
            }
            self.LoadingStatus(false);
        });
    };
    self.render();
}

function ProfileViewModel_Extend()
{
    var self = this,
        _accountName = arguments[0] || innTools.getURLParameterByName('accountname'),
        _profileProperties = 'FirstName,WorkEmail,CellPhone,HomePhone,WorkPhone,Fax,SPS-Location,SPS-Responsibility,SPS-Interests,AboutMe';
    self.profileProperties = ko.observable({});

    this.render = function ()
    {
        innTools.GetUserProfile({
            accountName: _accountName,
            callback: function (data, textStatus, jqXHR)
            {
                var _customPropertiesList = $.grep(data.d.UserProfileProperties.results, function (e) { return _profileProperties.indexOf(e.Key) > -1; });
                $.each(_customPropertiesList, function (i, item)
                {
                    self.profileProperties()[item.Key] = item.Value;
                });
            }
        });
    };
    this.render();
}

function ProfileViewModel(opts)
{
    var self = this;
    self.TwitterProfileHTML = ko.observable({}),
    self.LinkedInProfileHTML = ko.observable({});

    var _htmlTemp = '<a href="{0}" target="_blank">{1}<a/>';
    var _twitterHTML = String.format(_htmlTemp, $(opts.TwitterProfilePage).text(), $(opts.TwitterProfilePage).text()),
        _LinkedInHTML = String.format(_htmlTemp, $(opts.LinkedInProfile).text(), $(opts.LinkedInProfile).text());

    self.TwitterProfileHTML(_twitterHTML);
    self.LinkedInProfileHTML(_LinkedInHTML);
}

function AllAnnouncementsModel()
{
    var self = this,
        _ajaxData = {},
        lastDate = new Date(),
        _params = arguments[0],
        itemCount = 5,
        itemIndex = 0,
        _preloading = false;

    self.AnnouncementItems = ko.observableArray([]);
    self.errorText = ko.observable(false);
    self.isLastItem = ko.observable(true);
    self.loadingPanel = ko.observable(false);
    self.emptyPanel = ko.observable(false);
    self.RateOrder = ko.observable(false);

    _ajaxData["Filter"] = { "OrderType": "", "RateOrder": self.RateOrder() };

    self.render = function ()
    {
        if (_preloading) return false;

        _preloading = true;

        _ajaxData["NewestDate"] = lastDate;
        _ajaxData["ItemCount"] = itemCount;
        _ajaxData["Index"] = itemIndex;
        _ajaxData["Filter"]["RateOrder"] = self.RateOrder();

        $.ajax({
            type: 'POST',
            url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetLatestAnnouncements',
            data: innTools.JSONSerializer(_ajaxData),
            success: function (data, textStatus, jqXHR)
            {
                var _lastItem = data.Data.slice(-1);
                lastDate = (_lastItem.length > 0) ? _lastItem[0].Created : null;
                self.isLastItem((lastDate == null) ? false : true);
                itemIndex = itemIndex + itemCount;
                $.each(data.Data, function (i, item)
                {
                    self.AnnouncementItems.push(new AnnouncementItemModel(item));
                });
                self.loadingPanel(false);

                if (self.AnnouncementItems().length > 0)
                {
                    self.emptyPanel(false);
                } else
                {
                    self.emptyPanel(true);
                }

                _preloading = false;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                self.errorText(true);
            }
        });
    };

    self.onRateStatus = function (model, event)
    {
        self.AnnouncementItems.removeAll();

        var _rate = self.RateOrder();
        self.RateOrder(!_rate);

        lastDate = new Date();
        itemCount = 5;
        itemIndex = 0;

        self.render();
    };

    self.render();
}

function SurveyController(params)
{
    var self = this;
    self.Results = ko.observableArray([]);

    self.SurveyView = ko.observable(true);
    self.ResultView = ko.observable(false);
    self.ResultMessage = ko.observable(false);

    self.view1Sendbutton = ko.observable(false);
    self.view1Resultbutton = ko.observable(false);
    self.view2Sendbutton = ko.observable(false);
    self.view2Resultbutton = ko.observable(false);

    var loadingContainer = $(params.surveyModalContainer).find('.modal-body');

    //params.surveyModal.modal({ backdrop: 'static', keyboard: false });

    $.blockUI.defaults.message = RESOURCE.CLIENT_BlockUI_Message_Text;

    var isLoading = false;

    if (params.surverIsRespond)
    {
        self.SurveyView(false);
    } else
    {
        self.SurveyView(true);
    }
    var sonucButonu = params.surverIsRespond & params.surveyDisplayResult;
    var yanitlaButonu = !params.surverIsRespond;
    self.view1Resultbutton(sonucButonu);
    self.view2Resultbutton(sonucButonu);
    self.view1Sendbutton(yanitlaButonu);
    self.view2Sendbutton(yanitlaButonu);

    if (params.surveyDisplayResult && !params.surverIsRespond)
    {
        if (params.surverIsRespond)
        {
            self.SurveyView(false);
            self.ResultView(true);
        } else
        {
            self.SurveyView(true);
            self.ResultView(false);
        }
    }

    self.GetSurveyResponse = showSurveyResults;

    self.surveySendValue = function (e)
    {
        //params.SurveyResponse = params.surverResponseControl.find('input:checked').val();
        var selected = params.surverResponseControl.find('input:checked');

        if (!selected.length)
        {
            alert(RESOURCE.CLIENT_Survey_Alert_Text);
            return false;
        }

        var array = [];
        $.each(selected, function (i, item)
        {
            array.push($(this).attr('id').replace('surveyItem_', ''));
        });
        params.SurveyResponse = array.join();
        var requestData = String.format('SurveyID={0}&SurveyResponse={1}&SurveyStaticTitle={2}&RequestType=SubmitSurvey', params.surveyID, params.SurveyResponse, params.surveyStaticTitle);

        sendSurveyRespond(requestData);
    };

    self.showSurveyResults = showSurveyResults;

    self.showSurveyModal = function (event)
    {
        if (params.surverIsRespond)
        {
            self.SurveyView(false);
            self.ResultView(false);
            self.ResultMessage(true);
        } else
        {
            self.SurveyView(true);
            self.ResultView(false);
            self.ResultMessage(false);
        }

        params.surveyModal.modal('show');

        return false;
    };

    function sendSurveyRespond(request)
    {
        if (!isLoading)
        {
            loadingContainer.block();
            isLoading = true;
        }

        $.get(params.handler, request, function (d)
        {
            if (d.Output.Status == 1 && params.surveyDisplayResult)
            {
                params.surverIsRespond = true;
                self.view1Sendbutton(false);
                self.view2Sendbutton(false);
                self.view1Resultbutton(true);
                showSurveyResults();
            }
            else
            {
                if (isLoading)
                {
                    loadingContainer.unblock();
                    isLoading = false;
                }
                if (params.surveyDisplayResult)
                {
                    self.view1Sendbutton(false);
                    self.view2Sendbutton(false);

                    self.SurveyView(false);
                    self.ResultView(true);
                    self.ResultMessage(false);
                } else
                {
                    self.view1Sendbutton(false);
                    self.view2Sendbutton(false);

                    self.SurveyView(false);
                    self.ResultView(false);
                    self.ResultMessage(true);
                }
            }
        });
        return false;
    }

    function showSurveyResults(e)
    {
        params.surveyModal.modal('show');
        $("#surveyModal .modal-body").addClass('noBg');
        self.Results = [];

        if (isLoading)
        {
            loadingContainer.unblock();
            isLoading = false;
        }

        if (params.surveyDisplayResult && params.surverIsRespond)
        {
            if (!isLoading)
            {
                loadingContainer.block();
                isLoading = true;
            }
            $.getJSON(params.handler + "?SurveyID=" + SurveyControllerParams.surveyID, 'RequestType=ResponseResult&SurveyID=' + SurveyControllerParams.surveyID, function (f)
            {
                if (f.Output.Status === 1)
                {
                    $.each(f.ResponseList, function (index, item)
                    {
                        self.Results.push(item);
                    });
                    self.SurveyView(false);
                    self.ResultView(true);
                    self.ResultMessage(false);
                }
                else
                {
                    self.SurveyView(false);
                    self.ResultView(false);
                    self.ResultMessage(true);
                }
                if (isLoading)
                {
                    loadingContainer.unblock();
                    isLoading = false;
                }
            });
        } else
        {
            self.SurveyView(false);
            self.ResultView(false);
            self.ResultMessage(true);
        }
        return false;
    }

    self.setCookie = function ()
    {
        var surveyCookieName = String.format('MiniSurveyResponde_{0}', SurveyControllerParams.surveyID);
        $.cookie(surveyCookieName, true);
    };
}

function CommunityCompanyMain(params)
{

    var self = this;

    self.loading = ko.observable(false);

    self.Title = ko.observable(params.corpInfo.lbTitle),
    self.Address = ko.observable(params.corpInfo.lbAddress),
    self.AuthorizedUser = ko.observable(params.corpInfo.lbAuthorizedUser),
    self.PhoneNumber = ko.observable(params.corpInfo.lbTel),
    self.FaxNumber = ko.observable(params.corpInfo.lbFax),
    self.WebAddress = ko.observable(params.corpInfo.lbWebAddress),
    self.County = ko.observable(params.corpInfo.lbCountry);

    self.ddlChange = function (e)
    {
        var companyId;
        if (e.type == undefined)
        {
            companyId = innTools.getURLParameterByName('ID');


        } else if (e.type == "change")
        {
            companyId = $(e.currentTarget).val();
        }

        if (companyId == "") return false;

        self.loading(true);
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + String.format("/_api/web/lists/getbytitle('" + innTools.siteNames().Pages + "')/Items?$select=Title,AuthorizedUser,Address,PhoneNumber,FaxNumber,WebAddress&$filter=ID eq '{0}'", companyId), //substringof('Topluluk Şirketi',ContentType) and 
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" },
            success: function (data)
            {
                if (data.d.results)
                {
                    var dm = new CompanyDetailModel(data.d.results[0]);
                    self.Title(dm.Title);
                    self.AuthorizedUser(dm.AuthorizedUser);
                    self.Address(dm.Address);
                    self.PhoneNumber(dm.PhoneNumber);
                    self.FaxNumber(dm.FaxNumber);
                    self.WebAddress(dm.WebAddress);
                    self.County(dm.County);
                }
                self.loading(false);

                console.log("success", innTools.JSONSerializer(data));
            },
            error: function (xhr)
            {
                //console.log("error", innTools.JSONSerializer(xhr));
            }
        });

        return false;
    };

    if (params.ddlCompanies)
    {
        params.ddlCompanies.on('change', self.ddlChange);
    }

    self.ddlChange(window);

}

function MyWaitingTasksViewModel()
{
    var self = this;
    self.taskItems = ko.observableArray([]);
    self.taskKey = ko.observable("");

    self.taskKey.subscribe(function (value)
    {
        self.render();
    });
    self.render = function ()
    {
        $.post(_spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetMyWaitingTasks', innTools.JSONSerializer({ "TaskKey": self.taskKey() }), function (data, textStatus, jqXHR)
        {
            if (data.Data != null)
            {
                ko.mapping.fromJS(data.Data, {}, self.taskItems);
            }
        });
    };
    self.enterEvent = function (d, e)
    {
        if (e.keyCode === 13)
        {
            return false;
        }
        return true;
    };
    self.render();
}

ExecuteOrDelayUntilScriptLoaded(function ()
{
    var BlogAuthorHiddenField = $('.sp-peoplepicker-topLevel input[id*=BlogAuthor][type=hidden]'),
        BlogAuthorTextField = $('.sp-peoplepicker-topLevel input[id*=BlogAuthor][type=text]');

    BlogAuthorTextField.on('keydown', function (e)
    {
        var selectedPeople = eval(BlogAuthorHiddenField.val());
        if (selectedPeople !== undefined || $(selectedPeople).length)
        {
            if (selectedPeople[0].IsResolved)
            {
                e.preventDefault();
            }
        }
    });
}, 'clientpeoplepicker.js');

function myfunction()
{
    $.ajax({
        type: 'POST',
        url: _spPageContextInfo.webAbsoluteUrl + sa_config.portalBaseServiceUrl + '/GetMyWaitingTasks',

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        data: innTools.JSONSerializer({ 'TaskKey': 'dev' }),
        beforeSend: function ()
        {

        },
        success: function (data, textStatus, jqXHR)
        {
            console.log(data, textStatus, jqXHR);
        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR, textStatus, errorThrown);
        }
    });
}

