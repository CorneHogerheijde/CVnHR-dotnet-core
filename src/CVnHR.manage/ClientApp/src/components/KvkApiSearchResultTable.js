import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';

class KvkApiSearchResultTable extends Component {
    render() {
        const { items, totalItems, itemsPerPage, startPage } = this.props.kvkApiResult;
        const endPage = parseInt(totalItems / itemsPerPage) + (totalItems % itemsPerPage > 0 ? 1 : 0);

        return (
            <div className="kvk-api-result-table container">
                <p> {totalItems} items found.</p>

                {totalItems > 0 &&
                    items.map(item => {
                        const name = item.tradeNames === null ? '[??]' : (item.tradeNames.businessName || item.tradeNames.shortBusinessName);
                        const address = item.addresses ? item.addresses[0] : null;
                        const key = `${item.kvkNumber}-${item.rsin}-${address.street}-${address.houseNumber}`;

                        return <div className="row" key={key}>
                            <div className="col-sm-1">{item.kvkNumber}</div>
                            <div className="col-sm-5">{name}</div>
                            <div className="col-sm-4">{address && `${address.street} ${address.houseNumber}${address.houseNumberAddition} ${address.city}`}</div>
                            <div className="col-sm-1">
                                <Link to=''>zoek</Link>
                            </div>
                        </div>;
                    })
                }

                <div className="controls">
                    <span>{totalItems} resultaten gevonden. Pagina {startPage} van {endPage}</span>
                    {startPage > 1 && <Link to={`${this.props.router.location.pathname}?startPage=${startPage - 1}`}>vorige pagina</Link>}
                    {startPage < endPage && <Link to={`${this.props.router.location.pathname}?startPage=${startPage + 1}`}>volgende pagina</Link>}

                </div>

            </div>
        );
    }
}

export default connect(state => { return { router: state.router }; })(KvkApiSearchResultTable);