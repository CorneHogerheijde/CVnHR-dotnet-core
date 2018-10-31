import { compose, withPropsOnChange } from 'recompose';
import { withRouter } from 'react-router';
import queryString from 'query-string';

const propsWithQuery = withPropsOnChange(
    ['location', 'match'],
    ({ location, match }) => {
        return {
            location: {
                ...location,
                query: queryString.parse(location.search)
            },
            match
        };
    }
);

export default compose(withRouter, propsWithQuery);