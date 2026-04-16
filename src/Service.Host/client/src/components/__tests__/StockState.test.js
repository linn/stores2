import { describe, test, expect, vi, beforeEach } from 'vitest';
import '@testing-library/jest-dom';
import { screen, fireEvent } from '@testing-library/react';
import render from '../../test-utils';
import StockState from '../StockState';
import useGet from '../../hooks/useGet';
import usePut from '../../hooks/usePut';
import usePost from '../../hooks/usePost';

vi.mock('../../hooks/useGet', () => ({ default: vi.fn() }));
vi.mock('../../hooks/usePut', () => ({ default: vi.fn() }));
vi.mock('../../hooks/usePost', () => ({ default: vi.fn() }));

describe('StockState', () => {
    let mockGetSend;
    let mockUpdateSend;
    let mockCreateSend;

    beforeEach(() => {
        mockGetSend = vi.fn();
        mockUpdateSend = vi.fn();
        mockCreateSend = vi.fn();

        useGet.mockReturnValue({
            send: mockGetSend,
            isLoading: false,
            result: null
        });

        usePut.mockReturnValue({
            send: mockUpdateSend,
            isLoading: false,
            errorMessage: null,
            putResult: null
        });

        usePost.mockReturnValue({
            send: mockCreateSend,
            isLoading: false,
            errorMessage: null
        });
    });

    describe('when editing an existing stock state', () => {
        beforeEach(() => {
            useGet.mockReturnValue({
                send: mockGetSend,
                isLoading: false,
                result: { state: 'STORES', description: 'Good Stock', qcRequired: 'N' }
            });
        });

        test('renders the state field as disabled', () => {
            render(<StockState />);
            expect(screen.getByLabelText(/^state$/i)).toBeDisabled();
        });

        test('displays the loaded state value', () => {
            render(<StockState />);
            expect(screen.getByLabelText(/^state$/i)).toHaveValue('STORES');
        });

        test('displays the loaded description value', () => {
            render(<StockState />);
            expect(screen.getByLabelText(/description/i)).toHaveValue('Good Stock');
        });

        test('save button is disabled when no changes have been made', () => {
            render(<StockState />);
            expect(screen.getByRole('button', { name: /save/i })).toBeDisabled();
        });

        test('save button is enabled after changing the description', () => {
            render(<StockState />);
            fireEvent.change(screen.getByLabelText(/description/i), {
                target: { value: 'Updated Description' }
            });
            expect(screen.getByRole('button', { name: /save/i })).not.toBeDisabled();
        });

        test('calls update with updated form values when save is clicked', () => {
            render(<StockState />);
            fireEvent.change(screen.getByLabelText(/description/i), {
                target: { value: 'Updated Description' }
            });
            fireEvent.click(screen.getByRole('button', { name: /save/i }));
            expect(mockUpdateSend).toHaveBeenCalledWith(
                undefined,
                expect.objectContaining({
                    state: 'STORES',
                    description: 'Updated Description'
                })
            );
        });

        test('shows an error message when update fails', () => {
            usePut.mockReturnValue({
                send: mockUpdateSend,
                isLoading: false,
                errorMessage: 'Update failed',
                putResult: null
            });
            render(<StockState />);
            expect(screen.getByText(/update failed/i)).toBeInTheDocument();
        });

        test('cancel button resets description to original value', () => {
            render(<StockState />);
            fireEvent.change(screen.getByLabelText(/description/i), {
                target: { value: 'Changed Value' }
            });
            fireEvent.click(screen.getByRole('button', { name: /cancel/i }));
            expect(screen.getByLabelText(/description/i)).toHaveValue('Good Stock');
        });
    });

    describe('when creating a new stock state', () => {
        test('renders the state field as enabled', () => {
            render(<StockState creating />);
            expect(screen.getByLabelText(/^state$/i)).not.toBeDisabled();
        });

        test('save button is disabled when no changes have been made', () => {
            render(<StockState creating />);
            expect(screen.getByRole('button', { name: /save/i })).toBeDisabled();
        });

        test('converts state input to uppercase', () => {
            render(<StockState creating />);
            fireEvent.change(screen.getByLabelText(/^state$/i), {
                target: { value: 'stores' }
            });
            expect(screen.getByLabelText(/^state$/i)).toHaveValue('STORES');
        });

        test('save button is enabled after entering a state', () => {
            render(<StockState creating />);
            fireEvent.change(screen.getByLabelText(/^state$/i), {
                target: { value: 'NEW' }
            });
            expect(screen.getByRole('button', { name: /save/i })).not.toBeDisabled();
        });

        test('calls create with form values when save is clicked', () => {
            render(<StockState creating />);
            fireEvent.change(screen.getByLabelText(/^state$/i), {
                target: { value: 'NEW' }
            });
            fireEvent.change(screen.getByLabelText(/description/i), {
                target: { value: 'New State' }
            });
            fireEvent.click(screen.getByRole('button', { name: /save/i }));
            expect(mockCreateSend).toHaveBeenCalledWith(
                null,
                expect.objectContaining({ state: 'NEW', description: 'New State' })
            );
        });

        test('shows an error message when create fails', () => {
            usePost.mockReturnValue({
                send: mockCreateSend,
                isLoading: false,
                errorMessage: 'Create failed'
            });
            render(<StockState creating />);
            expect(screen.getByText(/create failed/i)).toBeInTheDocument();
        });

        test('cancel button resets the form', () => {
            render(<StockState creating />);
            fireEvent.change(screen.getByLabelText(/^state$/i), {
                target: { value: 'TEMP' }
            });
            fireEvent.click(screen.getByRole('button', { name: /cancel/i }));
            expect(screen.getByLabelText(/^state$/i)).toHaveValue('');
        });
    });
});
